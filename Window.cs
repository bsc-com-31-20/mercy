using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
namespace LearnOpenTK
{
    public class Window : GameWindow
    {
        private double _timeSinceStart;
        private readonly float[] _vertices =
        {
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, 
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f, 
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, 
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, 
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, 
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
        };
        private readonly Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);
        private int _vertexBufferObject;
        private int _vaoModel;
        private int _vaoLamp;
        private Shader _lampShader;
        private Shader _lightingShader;
        private Camera _camera;
        private bool _firstMove = true;
        private Vector2 _lastPos;
        private Vector3 _yellowCubePosition = new Vector3(6.0f, 0.0f, 0.0f); 
    private float _yellowCubeRotationSpeed = 30.0f;

    private Vector3 _blueCubePosition = new Vector3(9.0f, 0.0f, 0.0f); 
    private float _blueCubeRotationSpeed = 80.0f;
    
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }
        protected override void OnLoad()
{
    base.OnLoad();
    GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    GL.Enable(EnableCap.DepthTest);
    _vertexBufferObject = GL.GenBuffer();
    GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
    GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
    _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
    _lampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
    {
        _vaoModel = GL.GenVertexArray();
        GL.BindVertexArray(_vaoModel);
        var positionLocation = _lightingShader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        var normalLocation = _lightingShader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
    }
    {
        _vaoLamp = GL.GenVertexArray();
        GL.BindVertexArray(_vaoLamp);
        var positionLocation = _lampShader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
    }
    _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
    CursorState = CursorState.Grabbed;
    _lightingShader.Use();
    _lightingShader.SetVector3("lightColor", new Vector3(0.0f, 0.0f, 1.0f));
}
       protected override void OnRenderFrame(FrameEventArgs e)
{
    base.OnRenderFrame(e);
    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    float rotationSpeed = 30.0f; 
    float angle = rotationSpeed * (float)_timeSinceStart; 
    Matrix4 rotation = Matrix4.CreateFromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(angle));
    float yellowCubeAngle = _yellowCubeRotationSpeed * (float)_timeSinceStart; 
        Matrix4 yellowCubeRotation = Matrix4.CreateFromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(yellowCubeAngle));

        float blueCubeAngle = _blueCubeRotationSpeed * (float)_timeSinceStart; 
        Matrix4 blueCubeRotation = Matrix4.CreateFromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(blueCubeAngle));


    GL.BindVertexArray(_vaoModel);
    _lightingShader.Use();
    _lightingShader.SetMatrix4("model", rotation); 
    _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
    _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
    _lightingShader.SetVector3("objectColor", new Vector3(1.0f, 0.0f, 0.0f)); 
    _lightingShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));
    _lightingShader.SetVector3("lightPos", _lightPos);
    _lightingShader.SetVector3("viewPos", _camera.Position);
    GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
    GL.BindVertexArray(_vaoLamp);
    _lampShader.Use();
    Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
    lampMatrix = lampMatrix * Matrix4.CreateTranslation(_lightPos);
    _lampShader.SetMatrix4("model", lampMatrix);
    _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
    _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
    GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
    DrawYellowCube(yellowCubeRotation);
    DrawBlueCube(blueCubeRotation);
    SwapBuffers();
}
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
    _timeSinceStart += e.Time;
            if (!IsFocused)
            {
                return;
            }
            var input = KeyboardState;
            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            const float cameraSpeed = 3.5f;
            const float sensitivity = 0.8f;
            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; 
            }
            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; 
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; 
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; 
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; 
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; 
            }
            var mouse = MouseState;
            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov -= e.OffsetY;
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }
        private void DrawYellowCube(Matrix4 yellowCubeRotation)
{
    GL.BindVertexArray(_vaoModel);
    _lightingShader.Use();
    Matrix4 yellowCubeModel = Matrix4.CreateScale(0.6f);
    yellowCubeModel *= Matrix4.CreateTranslation(_yellowCubePosition);
    yellowCubeModel *= yellowCubeRotation; 
    _lightingShader.SetMatrix4("model", yellowCubeModel); 
    _lightingShader.SetVector3("objectColor", new Vector3(1.0f, 1.0f, 0.0f)); 
    _lightingShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f)); 
    GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
}

private void DrawBlueCube(Matrix4 yellowCubeRotation)
{
    // Define the radius and speed of the blue cube's orbit
    float orbitRadius = 3.0f; // Adjust as needed
    float orbitSpeed = 80.0f; // Adjust as needed

    // Calculate the angle of rotation for the blue cube around the yellow cube's orbit
    float blueCubeAngle = orbitSpeed * (float)_timeSinceStart; 

    // Calculate the position of the blue cube relative to the yellow cube's orbit
    Vector3 relativePosition = new Vector3(orbitRadius * (float)Math.Cos(blueCubeAngle), 0.0f, orbitRadius * (float)Math.Sin(blueCubeAngle));

    // Apply the rotation of the yellow cube
    relativePosition = Vector3.TransformPosition(relativePosition, yellowCubeRotation);

    // Define the model matrix for the blue cube
    Matrix4 blueCubeModel = Matrix4.CreateScale(0.3f);
    blueCubeModel *= Matrix4.CreateTranslation(_yellowCubePosition + relativePosition);

    // Set the model matrix and other shader parameters
    GL.BindVertexArray(_vaoModel);
    _lightingShader.Use();
    _lightingShader.SetMatrix4("model", blueCubeModel); 
    _lightingShader.SetVector3("objectColor", new Vector3(0.0f, 0.0f, 1.0f)); 
    _lightingShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f)); 
    GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
}



    }
}
