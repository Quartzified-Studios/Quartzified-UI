using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Quartzified.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class AdvancedButton : Selectable, IPointerClickHandler
    {
        [Header("Tap and Hold")]
        [Tooltip("Enable Tap and Hold!\n(Only works with Left Click Events)")]
        public bool tapAndHold;

        [Space]
        [Tooltip("Time the button needs to be held for before initializing hold cycle.\n(Usually sooner than [Fast Tap Delay]")]
        public float initialTapDelay = 0;
        [Tooltip("Time waited between every hold cycle.\n(Usually greater than [Fast Hold Delay]")]
        public float holdDelay = 0.5f;

        [Space]
        [Tooltip("Time the button needs to be held for before initializing quick hold cycle.\n(Usually greater than [Initial Tap Delay]")]
        public float fastTapDelay = 0.4f;
        [Tooltip("Time waited between every quick hold cycle.\n(Usually lower than [Hold Delay]")]
        public float fastHoldDelay = 0.2f;

        [Space]
        public UnityEvent OnClick;
        public UnityEvent OnRightClick;

        // Tap and Hold States
        ButtonState tapHoldState = ButtonState.None;
        float tapHoldTimer = 0;
        float fastTapTimer = 0;

        void Update()
        {
            HandleTapAndHold();
        }

        public void PressLeft()
        {

            if (!IsActive() || !IsInteractable())
                return;

            OnClick?.Invoke();
        }

        public void PressRight()
        {
            if (!IsActive() || !IsInteractable())
                return;

            OnRightClick?.Invoke();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            // Check if we are currently in Hold State
            // If we are we need to skip the input as it would register when letting go of the button
            if (tapHoldState == ButtonState.HoldDelay)
                return;

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                PressLeft();
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                PressRight();
                return;
            }

        }


        void HandleTapAndHold()
        {
            // Check that we want to call hold Event
            if (tapAndHold)
            {
                // Check that we are currently holding down the button
                if (IsPressed())
                {
                    // Use Time.unscaledDeltaTime to not be affected by TimeScale
                    float time = Time.unscaledDeltaTime;

                    // If Button State is None Trigger the Initial Tap Delay
                    if (tapHoldState == ButtonState.None)
                    {
                        tapHoldTimer = initialTapDelay;
                        tapHoldState = ButtonState.InitialTapDelay;
                    }
                    // If Button State is InitialTapDelay wait out the first delay to Trigger Hold & Trigger First Press
                    else if (tapHoldState == ButtonState.InitialTapDelay)
                    {
                        tapHoldTimer -= time;
                        if (tapHoldTimer <= 0)
                        {
                            tapHoldState = ButtonState.HoldDelay;
                            tapHoldTimer = holdDelay;
                            fastTapTimer = fastTapDelay;

                            PressLeft();
                        }
                    }
                    // If Button State is HoldDelay cycle through the tap delays to Trigger Press
                    else if (tapHoldState == ButtonState.HoldDelay)
                    {
                        tapHoldTimer -= time;
                        fastTapTimer -= time;

                        while (tapHoldTimer <= 0)
                        {
                            PressLeft();

                            if (fastTapTimer <= 0)
                                tapHoldTimer += fastHoldDelay;
                            else
                                tapHoldTimer += holdDelay;
                        }
                    }
                }
                else
                {
                    tapHoldState = ButtonState.None;
                }
            }

        }
    }
}