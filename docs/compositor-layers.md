# Using Pico transmission layers

## Transmission layers explained

Transmission layers, sometimes described as Compositor layers or a Synthesis layer, is a different way of rendering to VR headsets.

Normal rendering will have an applications content rendered onto an 'eye buffer', and then the 'eye buffer' will be sampled by ATW (Asynchronous TimeWarp) to find out what content needs to be re-rendered onto a screen, and then finally it will actually be rendered onto a VR screen.
