describe('Man Walking on Road AR', () => {
    beforeEach(() => {
      // Set up a test fixture with the AR scene and marker
      document.body.innerHTML = `
        <div id="test-container">
          <a-scene embedded arjs>
            <a-marker preset="custom" type="pattern" url="marker.patt">
              <a-entity position="0 0 0">
                <a-entity id="man" gltf-model="#man-model"></a-entity>
                <a-entity id="road" gltf-model="#road-model"></a-entity>
              </a-entity>
            </a-marker>
            <a-entity camera></a-entity>
          </a-scene>
        </div>
      `;
    });
  
    test('Man and road models are loaded and positioned correctly', () => {
      // Wait for the models to load and for the AR session to start
      return new Promise((resolve) => {
        window.addEventListener('arjs-nft-loaded', () => {
          setTimeout(resolve, 1000);
        });
      }).then(() => {
        const man = document.querySelector('#man');
        const road = document.querySelector('#road');
  
        // Verify that the man and road models are present
        expect(man).not.toBeNull();
        expect(road).not.toBeNull();
  
        // Verify that the man is positioned correctly
        expect(man.getAttribute('position').x).toBeCloseTo(0, 2);
        expect(man.getAttribute('position').y).toBeCloseTo(0.25, 2);
        expect(man.getAttribute('position').z).toBeCloseTo(-2.5, 2);
  
        // Verify that the road is positioned correctly
        expect(road.getAttribute('position').x).toBeCloseTo(0, 2);
        expect(road.getAttribute('position').y).toBeCloseTo(-0.1, 2);
        expect(road.getAttribute('position').z).toBeCloseTo(-2, 2);
      });
    });
  });
  