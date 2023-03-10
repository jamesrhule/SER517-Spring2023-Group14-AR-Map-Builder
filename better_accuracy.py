import cv2
import numpy as np

# Load YOLOv3 weights and configuration
net = cv2.dnn.readNet("yolov3.weights", "yolov3.cfg")

# Load classes
classes = []
with open("coco.names", "r") as f:
    classes = [line.strip() for line in f.readlines()]

# Set input size
net.setInputSize(416, 416)

# Set input mean and scaling
mean = (0, 0, 0)
scale = 0.00392

# Load image
image = cv2.imread("image.jpg")

# Create blob from image
blob = cv2.dnn.blobFromImage(image, scale, (416, 416), mean, True, crop=False)

# Set input blob for the network
net.setInput(blob)

# Forward pass to get output
outs = net.forward(net.getUnconnectedOutLayersNames())

# Get confidence threshold and non-maximum suppression threshold
confThreshold = 0.5
nmsThreshold = 0.4

# Iterate over each output layer and detect objects
for out in outs:
    for detection in out:
        scores = detection[5:]
        classId = np.argmax(scores)
        confidence = scores[classId]
        if confidence > confThreshold:
            center_x = int(detection[0] * image.shape[1])
            center_y = int(detection[1] * image.shape[0])
            width = int(detection[2] * image.shape[1])
            height = int(detection[3] * image.shape[0])
            left = int(center_x - width / 2)
            top = int(center_y - height / 2)
            # Draw bounding box and label
            cv2.rectangle(image, (left, top), (left + width, top + height), (0, 255, 0), 3)
            cv2.putText(image, f'{classes[classId]} {confidence:.2f}', (left, top - 10), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 255, 0), 2)

# Apply non-maximum suppression
indices = cv2.dnn.NMSBoxes(bboxes, confidences, confThreshold, nmsThreshold)

# Display image
cv2.imshow("Image", image)
cv2.waitKey(0)
cv2.destroyAllWindows()
