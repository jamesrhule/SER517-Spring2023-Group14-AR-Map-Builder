#!/bin/python

# ImageAI Github: https://github.com/OlafenwaMoses/ImageAI

from imageai.Classification import ImageClassification
from os import listdir

DENSENET_PATH = "./models/densenet121-a639ec97.pth"
INCEPTION_PATH = "./models/inception_v3_google-1a9a5a14.pth"
RESNET_PATH = "./models/resnet50-19c8e357.pth"
MOBILENET_PATH = "./models/mobilenet_v2-b0353104.pth"
IMGS_DIR = "./imgs"

class Color:
    HEADER = '\033[95m'
    OKBLUE = '\033[94m'
    OKGREEN = '\033[92m'
    WARNING = '\033[93m'
    FAIL = '\033[91m'
    ENDC = '\033[0m'
    BOLD = '\033[1m'
    UNDERLINE = '\033[4m'

if __name__ == "__main__":
    predictionModelDenseNet = ImageClassification()
    predictionModelDenseNet.setModelTypeAsDenseNet121()
    predictionModelDenseNet.setModelPath(DENSENET_PATH)
    predictionModelDenseNet.loadModel()

    predictionModelInception = ImageClassification()
    predictionModelInception.setModelTypeAsInceptionV3()
    predictionModelInception.setModelPath(INCEPTION_PATH)
    predictionModelInception.loadModel()

    resnetModel = ImageClassification()
    resnetModel.setModelTypeAsResNet50()
    resnetModel.setModelPath(RESNET_PATH)
    resnetModel.loadModel()

    mobileNetModel = ImageClassification()
    mobileNetModel.setModelTypeAsMobileNetV2()
    mobileNetModel.setModelPath(MOBILENET_PATH)
    mobileNetModel.loadModel()

    models = [
        (DENSENET_PATH, predictionModelDenseNet), 
        (INCEPTION_PATH, predictionModelInception),
        (RESNET_PATH, resnetModel),
        (MOBILENET_PATH, mobileNetModel)
    ]

    for path in listdir(IMGS_DIR):
        relativePath = f"{IMGS_DIR}/{path}"
        for model in models:
            print(f"{Color.OKGREEN}Results for '{relativePath}' {Color.BOLD}(using '{model[0]}'){Color.ENDC}:")
            predictions, probabilities = model[1].classifyImage(relativePath, result_count=10)
            for prediction, probability in zip(predictions, probabilities):
                print(f"    {prediction}: {probability}%")
