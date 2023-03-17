#!/bin/python

# ImageAI Github: https://github.com/OlafenwaMoses/ImageAI

from imageai.Detection import ObjectDetection
from predict import IMGS_DIR, Color
from os import listdir

RETINANET_PATH = "./models/retinanet_resnet50_fpn_coco-eeacb38b.pth"

if __name__ == "__main__":
    detectorRetinaNetModel = ObjectDetection()

    detectorRetinaNetModel.setModelTypeAsRetinaNet()
    detectorRetinaNetModel.setModelPath(RETINANET_PATH)
    detectorRetinaNetModel.loadModel()

    models = [
        (RETINANET_PATH, detectorRetinaNetModel, "retinanet")
    ]

    for path in listdir(IMGS_DIR):
        relativePath = f"{IMGS_DIR}/{path}"
        for model in models:
            print(f"{Color.OKGREEN}Results for '{relativePath}' {Color.BOLD}(using '{model[0]}'){Color.ENDC}:")
            detections = model[1].detectObjectsFromImage(input_image=relativePath, 
                                                        output_image_path=f"./output/{model[2]}_{path}", 
                                                        minimum_percentage_probability=30)
            for detection in detections:
                print(f"    {detection['name']}: {detection['percentage_probability']}%")
