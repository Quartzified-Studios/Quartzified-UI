using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Quartzified.Audio;

#if UNITY_EDITOR
using UnityEditor;
using TMPro;
#endif

namespace Quartzified.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SimpleButton : Selectable, IPointerClickHandler
    {
        [Header("Tap & Hold")]
        public bool tapAndHold;
        public float initialTapDelay = 0;
        public float holdDelay = 0.5f;
        public float fastTapDelay = 0.2f;
        public float fastTapHoldDelay = 0.1f;

        [Header("Sound Effects")]
        public AudioClip soundEffect;
        public float soundEffectCooldown = .1f;
        float sfxCooldown;

        [Header("Events")]
        public UnityEvent OnClicked;

        #region Tap & Hold Magic
        ButtonState tapHoldState = ButtonState.None;
        float tapHoldTimer = 0;
        float fastTapTimer = 0;

        #endregion

        void Update()
        {
            if (sfxCooldown > 0)
                sfxCooldown -= Time.deltaTime;

            ButtonTapAndHold();
        }

        void ButtonPress()
        {
            if (IsActive() && interactable)
            {
                PlaySoundEffect();
                OnClicked?.Invoke();
            }
        }

        void ButtonTapAndHold()
        {
            if (tapAndHold)
            {
                if (IsPressed() && interactable)
                {
                    float time = Time.unscaledDeltaTime;
                    if (tapHoldState == ButtonState.None)
                    {
                        tapHoldTimer = initialTapDelay;
                        tapHoldState = ButtonState.InitialTapDelay;
                    }
                    else if (tapHoldState == ButtonState.InitialTapDelay)
                    {
                        tapHoldTimer -= time;
                        if (tapHoldTimer <= 0)
                        {
                            tapHoldState = ButtonState.HoldDelay;
                            tapHoldTimer = holdDelay;
                            fastTapTimer = fastTapDelay;

                            ButtonPress();
                        }
                    }
                    else if (tapHoldState == ButtonState.HoldDelay)
                    {
                        tapHoldTimer -= time;
                        fastTapTimer -= time;

                        while (tapHoldTimer <= 0)
                        {
                            ButtonPress();

                            if (fastTapTimer <= 0)
                                tapHoldTimer += fastTapHoldDelay;
                            else
                                tapHoldTimer += holdDelay;
                        }
                    }
                }
                else
                    tapHoldState = ButtonState.None;
            }
        }

        void PlaySoundEffect()
        {
            if (sfxCooldown <= 0)
            {
                if (IsActive())
                {
                    if (soundEffect != null)
                        PlayEffectSound(soundEffect);

                    sfxCooldown = soundEffectCooldown;
                }
            }
        }

        void PlayEffectSound(AudioClip clip)
        {
            if (UniversalAudioSource.instance != null)
                UniversalAudioSource.instance.effectLayer.PlayEffect(clip);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (tapHoldState != ButtonState.HoldDelay)
            {
                ButtonPress();
            }
        }

        public void SetHoldDelays(float tapsPerSecond)
        {
            holdDelay = Mathf.Max(1f / tapsPerSecond, 0.01f);
            fastTapHoldDelay = Mathf.Max(1f / tapsPerSecond, 0.01f);
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Convert/Button (UI)/Simple Button", false, 7)]
        static void ConvertToSimpleButton(MenuCommand menuCommand)
        {
            GameObject go = menuCommand.context as GameObject;
            Button button = go.GetComponent<Button>();

            if (button != null)
            {
                // Temp Collect for Convert
                bool interactable = button.interactable;
                Transition transition = button.transition;
                SpriteState spriteState = button.spriteState;
                ColorBlock colors = button.colors;

                GameObject holderObj = button.gameObject;

                // Destroy old Button Component
                DestroyImmediate(button);

                SimpleButton newButton = holderObj.AddComponent<SimpleButton>();
                newButton.interactable = interactable;
                newButton.transition = transition;
                newButton.spriteState = spriteState;
                newButton.colors = colors;
            }
        }

        [MenuItem("GameObject/UI/Simple Button")]
        static void CreateSimpleButton(MenuCommand menuCommand)
        {
            // Develope Main Object
            GameObject go = new GameObject("Simple Button", typeof(RectTransform), typeof(Image), typeof(SimpleButton));
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            go.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 32);
            go.GetComponent<SimpleButton>().image = go.GetComponent<Image>();


            // Develope Child Object (Text)
            GameObject child = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
            GameObjectUtility.SetParentAndAlign(child, go);

            TextMeshProUGUI childText = child.GetComponent<TextMeshProUGUI>();
            childText.text = "New Text";
            childText.color = Color.black;
            childText.alignment = TextAlignmentOptions.Center;

            RectTransform childRect = child.GetComponent<RectTransform>();
            childRect.anchorMax = new Vector2(1, 1);
            childRect.anchorMin = new Vector2(0, 0);
            childRect.sizeDelta = new Vector2(0, 0);

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
#endif
    }

}
