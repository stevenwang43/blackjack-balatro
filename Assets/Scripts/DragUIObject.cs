using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIObject : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    public GameObject highlightObject;
    public HandManager handManager;
    public UIManager uiManager;
    public float movementSensitivity = 1.0f; // Adjustable sensitivity
    public float hoverHeight = 60f; // How much to move the card up on hover
    private Vector3 returnPosition; // Store the original position for resetting on hover exit
    public float hoverMoveSpeed = 5f; // Speed of hover transition

    private bool isDragging = false; // Flag to indicate if the object is being dragged

    private Canvas cardCanvas; // The canvas attached to the card

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        returnPosition = rectTransform.localPosition; // Store the card's original position

        // Find the HandManager in the parent hierarchy
        handManager = GetComponentInParent<HandManager>();
        uiManager = FindAnyObjectByType<UIManager>();

        // Ensure the highlight object is initially inactive
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }

        // Get the Canvas attached to the card's RectTransform
        cardCanvas = GetComponentInChildren<Canvas>();
    }

    public void SetPosition(Vector3 newPosition)
    {
        returnPosition = newPosition; // Update the original position
        rectTransform.localPosition = newPosition; // Set the new position to the RectTransform
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (handManager != null && handManager.name == "PlayerHand") {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
            originalPanelLocalPosition = rectTransform.localPosition;

            // Set the dragging flag to true
            isDragging = true;

            // Ensure the highlight object is visible when dragging
            if (highlightObject != null)
            {
                highlightObject.SetActive(true); // Show the highlight object
            }

            // Lift the card slightly when dragging starts
            StopAllCoroutines();
            StartCoroutine(SmoothHoverTransition(returnPosition + new Vector3(0, hoverHeight, 0)));

            // Set sorting order of the Canvas to a high value to make it appear on top during drag
            if (cardCanvas != null)
            {
                cardCanvas.sortingOrder = 1;  // Temporarily move to the top when dragging
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (handManager != null && handManager.name == "PlayerHand") {
            // Get the position of the pointer in screen space
            Vector2 screenPointerPosition = eventData.position;

            // Convert the screen position directly to the local position in the canvas
            Vector2 localPointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPointerPosition, eventData.pressEventCamera, out localPointerPosition);

            // Calculate the offset only along the X-axis
            float offsetX = localPointerPosition.x - originalLocalPointerPosition.x;

            // Update the position of the object, snapping the center to the cursor on the X-axis
            rectTransform.localPosition = new Vector3(originalPanelLocalPosition.x + offsetX, originalPanelLocalPosition.y, originalPanelLocalPosition.z);

            // After dragging, notify the HandManager to reorder cards
            if (handManager != null)
            {
                handManager.ReorderCards();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Only handle hover logic if we're not dragging the object
        if (!isDragging)
        {
            // Smoothly move the card slightly up when hovered over
            if (highlightObject != null)
            {
                highlightObject.SetActive(true); // Show the highlight object

                // Smooth transition of card's Y position upwards
                StopAllCoroutines(); // Stop any ongoing transition
                StartCoroutine(SmoothHoverTransition(returnPosition + new Vector3(0, hoverHeight, 0)));
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Only handle hover reset logic if we're not dragging the object
        if (!isDragging)
        {
            // Smoothly reset the card back to its original position when hover ends
            if (highlightObject != null)
            {
                highlightObject.SetActive(false); // Hide the highlight object
                StopAllCoroutines(); // Stop any ongoing transition
                StartCoroutine(SmoothHoverTransition(returnPosition));
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Set the dragging flag to false when the drag ends
        isDragging = false;

        // Smoothly return to the original position when the object is released
        StopAllCoroutines(); // Stop any ongoing transition
        StartCoroutine(SmoothReturnToOriginalPosition(returnPosition));

        // Optionally hide the highlight object when dragging ends
        if (highlightObject != null)
        {
            highlightObject.SetActive(false); // Hide the highlight object when not dragging
        }
        if (handManager != null) {
            ResetCardSortingOrder();
        }
    }

    public void ResetCardSortingOrder()
    {
        // Loop through all cards and set their sorting order
        for (int i = 0; i < handManager.cardsInHand.Count; i++)
        {
            GameObject card = handManager.cardsInHand[i];
            Canvas cardCanvas = card.GetComponentInChildren<Canvas>();  // Get the canvas of the card
            
            // Set the sorting order based on the position in the hand
            cardCanvas.sortingOrder = -i;  // Use the index in the list as the sorting order
        }
        uiManager.UpdateGameplayUI();
    }

    // Coroutine to smoothly transition between positions
    private IEnumerator SmoothHoverTransition(Vector3 targetPosition)
    {
        Vector3 currentPosition = rectTransform.localPosition;
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            rectTransform.localPosition = Vector3.Lerp(currentPosition, targetPosition, timeElapsed);
            timeElapsed += Time.deltaTime * hoverMoveSpeed;
            yield return null;
        }

        rectTransform.localPosition = targetPosition; // Ensure we reach the target position
    }

    // Coroutine to smoothly return to the original position
    private IEnumerator SmoothReturnToOriginalPosition(Vector3 targetPosition)
    {
        Vector3 currentPosition = rectTransform.localPosition;
        float timeElapsed = 0f;

        // Smoothly move the object back to the original position
        while (timeElapsed < 1f)
        {
            rectTransform.localPosition = Vector3.Lerp(currentPosition, targetPosition, timeElapsed);
            timeElapsed += Time.deltaTime * hoverMoveSpeed;
            yield return null;
        }

        rectTransform.localPosition = targetPosition; // Ensure we reach the target position
    }
}
