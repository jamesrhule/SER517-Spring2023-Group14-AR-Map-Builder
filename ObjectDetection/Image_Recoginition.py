import numpy as np
from keras.preprocessing import image
from keras.applications import vgg16

# Load the pre-trained model
model = vgg16.VGG16(weights='imagenet')

# Load the image and resize it to (224, 224) which is the input size for the VGG16 model
img = image.load_img('path/to/image.jpg', target_size=(224, 224))

# Convert the image to a numpy array
img_array = image.img_to_array(img)

# Expand the dimensions of the array to match the input shape of the model
img_array = np.expand_dims(img_array, axis=0)

# Preprocess the image (normalize pixel values and convert from RGB to BGR)
img_array = vgg16.preprocess_input(img_array)

# Use the model to predict the label of the image
predictions = model.predict(img_array)

# Decode the predictions to obtain the human-readable label
label = vgg16.decode_predictions(predictions, top=1)[0][0]

# Print the label
print(label[1])
