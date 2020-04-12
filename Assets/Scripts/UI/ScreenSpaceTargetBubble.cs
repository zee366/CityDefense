using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI {
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreenSpaceTargetBubble : MonoBehaviour {

        public  RectTransform               bubbleFrame;
        public  AudioClip                   openSound;
        public  Vector4                     framePadding; // Left, Top, Right, Bottom
        public  Transform                   inputIcon;
        private RectTransform               _frameContent;
        private Vector3                     _lastPos;
        private CanvasGroup                 _group;
        private CoroutineExecutor.TaskState _coroHandle;
        private Vector3                     _initialScale;

        private float _transitionSpeed = 3f;

        private bool _opened       = false;
        private bool _inTransition = false;

        public bool Opened {
            get { return _opened; }
        }

        public bool InTransition {
            get { return _inTransition; }
        }


        private void Awake() {
            _group = GetComponent<CanvasGroup>();
            bubbleFrame.gameObject.SetActive(false);
            _lastPos      = bubbleFrame.transform.position;
            _initialScale = bubbleFrame.transform.localScale;
        }


        private void Update() { UpdatePositionOnScreen(); }


        public void Open() {
            if ( _coroHandle != null ) _coroHandle.Stop();
            bubbleFrame.gameObject.SetActive(true);

            _inTransition = true;
            _coroHandle   = CoroutineExecutor.CreateTask(OpenTransition());
            _coroHandle.Start();

            RandomSoundPlayer.PlaySoundFx(openSound, 0.3f);
        }


        public void Close() {
            if ( _coroHandle != null ) _coroHandle.Stop();

            _inTransition = true;
            _coroHandle   = CoroutineExecutor.CreateTask(CloseTransition());
            _coroHandle.Start();
        }


        private IEnumerator OpenTransition() {
            float t = 0;
            while ( true ) {
                bubbleFrame.transform.localScale =  _initialScale * EasingFunction.EaseOutElastic(0, 1, t);
                _group.alpha                     =  EasingFunction.EaseOutElastic(0, 1, t);
                t                                += Time.deltaTime * _transitionSpeed;

                if ( t > 1.1f ) break;

                yield return null;
            }

            _opened       = true;
            _inTransition = false;
        }


        private IEnumerator CloseTransition() {
            float t = 0;
            while ( true ) {
                bubbleFrame.transform.localScale =  _initialScale * EasingFunction.EaseInElastic(1, 0, t);
                _group.alpha                     =  EasingFunction.EaseInElastic(1, 0, t);
                t                                += Time.deltaTime * _transitionSpeed;

                if ( t > 1.1f ) break;

                yield return null;
            }

            _opened       = false;
            _inTransition = false;
            bubbleFrame.gameObject.SetActive(false);
        }


        public void SetPadding(Vector4 sidesPadding) { framePadding = sidesPadding; }


        public void SetContent(RectTransform content) {
            CleanContent();

            Rect  frameRect = bubbleFrame.rect;
            float midHeight = (frameRect.height - framePadding.y - framePadding.w) / 2.0f + framePadding.w;

            content.transform.SetParent(bubbleFrame);
            content.localPosition = new Vector3(0, midHeight, 0);
            content.localScale    = Vector3.one;
            // Apply Padding
            content.sizeDelta = new Vector2(frameRect.width - framePadding.x - framePadding.z,
                                            frameRect.height - framePadding.y - framePadding.w);


            // Ensure to keep inner images clean and with correct aspect
            Image[] imagesComp = content.GetComponents<Image>();
            foreach ( Image imgCmp in imagesComp )
                imgCmp.preserveAspect = true;
        }


        public void SetFramingScale(float s) { bubbleFrame.localScale = new Vector3(s, s, 1); }

        public void SetFramingImage(Sprite newImage) { bubbleFrame.GetComponent<Image>().sprite = newImage; }


        private void UpdatePositionOnScreen() {
            if ( Camera.main == null ) return;

            Vector3 anchorScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            anchorScreenPos = WorldToScreenPointProjected(Camera.main, transform.position);

            // Vector3 playerPos       = PlayerController.GetLocalPlayerReference().transform.position;
            // Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(playerPos);
            //
            // Vector3 direction = playerScreenPos - anchorScreenPos;
            // float   angle     = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Vector2 finalScreenPos = new Vector2(anchorScreenPos.x, anchorScreenPos.y);

            // Clamp to screen
            if ( finalScreenPos.x >= Screen.width ) finalScreenPos.x = Screen.width;
            if ( finalScreenPos.x <= 0 ) finalScreenPos.x            = 0f;

            if ( finalScreenPos.y >= Screen.height ) finalScreenPos.y = Screen.height;
            if ( finalScreenPos.y <= 0 ) finalScreenPos.y             = 0f;


            Vector3 wantedPos = new Vector3(finalScreenPos.x, finalScreenPos.y, 0);
            // bubbleFrame.position = Vector3.MoveTowards(_lastPos, wantedPos, 2 * Time.deltaTime);
            bubbleFrame.position = wantedPos;
            bubbleFrame.rotation = Quaternion.Euler(0, 0, 0);

            _lastPos = bubbleFrame.position;
        }


        public static Vector2 WorldToScreenPointProjected(Camera camera, Vector3 worldPos) {
            Vector3 camNormal     = camera.transform.forward;
            Vector3 vectorFromCam = worldPos - camera.transform.position;
            float   camNormDot    = Vector3.Dot(camNormal, vectorFromCam);
            if ( camNormDot <= 0 ) {
                // we are behind the camera forward facing plane, project the position in front of the plane
                Vector3 proj = (camNormal * camNormDot * 1.01f);
                worldPos = camera.transform.position + (vectorFromCam - proj);
            }

            return RectTransformUtility.WorldToScreenPoint(camera, worldPos);
        }


        private void CleanContent() {
            // Clean childs
            foreach ( Transform c in bubbleFrame.transform ) {
                if ( c.Equals(inputIcon) ) continue;

                Destroy(c.gameObject);
            }
        }

    }
}