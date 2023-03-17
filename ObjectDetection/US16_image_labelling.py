mport cv2

# Load the image to be recognized
img = cv2.imread('image.jpg')

# Load the pre-trained model for image recognition
model = cv2.dnn.readNetFromCaffe('deploy.prototxt', 'model.caffemodel')

# Define the labels for the output classes
labels = ['cat', 'dog', 'bird']

# Create a blob from the image to be recognized
blob = cv2.dnn.blobFromImage(cv2.resize(img, (224, 224)), 1.0, (224, 224), (104.0, 117.0, 123.0))

# Set the input for the model
model.setInput(blob)

# Run a forward pass through the network to obtain the predictions
preds = model.forward()

# Find the index of the class with the highest probability
idx = preds[0].argmax()

# Assign the corresponding label to the image
label = labels[idx]

# Display the label on the image
cv2.putText(img, label, (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 0.9, (0, 255, 0), 2)

# Display the image with the assigned label
cv2.imshow('Image with label', img)
cv2.waitKey(0)
cv2.destroyAllWindows()
