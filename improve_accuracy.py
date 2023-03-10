import torch
import cv2
import numpy as np

# Load YOLOv5 model
model = torch.hub.load('ultralytics/yolov5', 'yolov5s', pretrained=True)

# Set confidence and non-maximum suppression thresholds
conf_threshold = 0.5
nms_threshold = 0.4

# Load image
image = cv2.imread("image.jpg")

# Perform detection
results = model(image)

# Iterate over each detected object
for detection in results.pred:
    # Get class index and confidence score
    class_index = detection[:, 5].argmax().item()
    confidence = detection[:, 5].max().item()

    # Check if confidence is greater than threshold
    if confidence > conf_threshold:
        # Get bounding box coordinates
        xmin, ymin, xmax, ymax = detection[:, :4][0].tolist()

        # Draw bounding box and label
        cv2.rectangle(image, (int(xmin), int(ymin)), (int(xmax), int(ymax)), (0, 255, 0), 3)
        cv2.putText(image, f'{model.names[class_index]} {confidence:.2f}', (int(xmin), int(ymin - 10)), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 2)

# Apply non-maximum suppression
results.pred = [detections[torchvision.ops.nms(detections[:, :4], detections[:, 5], nms_threshold)] for detections in results.pred]

# Display image
cv2.imshow("Image", image)
cv2.waitKey(0)
cv2.destroyAllWindows()
