// Copyright (c) 2022 8th Wall, Inc.
//
// app.js is the main entry point for your 8th Wall app. Code here will execute after head.html
// is loaded, and before body.html is loaded.

import './css/index.css'
import {placeItHereButtonComponent} from './components/placeItHereButton.js'
import {tapPlaceCursorComponent} from './components/tap-place-cursor'

AFRAME.registerComponent('placeithere-button', placeItHereButtonComponent)
AFRAME.registerComponent('tap-place-cursor', tapPlaceCursorComponent)

import {cubeEnvMapComponent} from './components/cubemap-static'
AFRAME.registerComponent('cubemap-static', cubeEnvMapComponent)

import {cubeMapRealtimeComponent} from './components/cubemap-realtime'
AFRAME.registerComponent('cubemap-realtime', cubeMapRealtimeComponent)

import {resetButtonComponent} from './components/resetButton.js'
AFRAME.registerComponent('reset-button', resetButtonComponent)


AFRAME.registerComponent('no-cull', {
  init() {
    this.el.addEventListener('model-loaded', () => {
      this.el.object3D.traverse(obj => obj.frustumCulled = false)
    })
  },
})
