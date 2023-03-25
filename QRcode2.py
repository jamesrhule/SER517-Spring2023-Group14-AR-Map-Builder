import cv2
import numpy as np
from pyzbar.pyzbar import decode

cap = cv2.VideoCapture(0)

while True:
    _, frame = cap.read()

    # decode QR code from the frame
    decoded_objs = decode(frame)

    # loop over the decoded objects
    for obj in decoded_objs:
        print("Data:", obj.data.decode())

    cv2.imshow("frame", frame)

    # exit if 'q' is pressed
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
