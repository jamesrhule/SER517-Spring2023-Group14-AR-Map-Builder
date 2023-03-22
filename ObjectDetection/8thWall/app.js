// Copyright (c) 2021 8th Wall, Inc.
//
// app.js is the main entry point for your 8th Wall app. Code here will execute after head.html
// is loaded, and before body.html is loaded.

import {targetVideoComponent} from './target-video'

AFRAME.registerComponent('target-video', targetVideoComponent)
