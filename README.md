

# QUARANTINE ZONE


## **Criadores**

Gilmar Correia Jeronimo        - RA: 11014515

Ivan Correia Lima Coqueiro     - RA: 11026416

Lucas Barboza Moreira Pinheiro - RA: 11017015



## **Descrição:** 

Se prepare para imergir neste TopDown Shooter como um Agente policial que busca provas de um crime, mas se depara com algo muito pior. Baixe o jogo em: https://github.com/lucaslbmp/Shoot-Game/releases/tag/v.1


<html lang="en-us">
  <head>
    <meta charset="utf-8">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Unity WebGL Player | Quarantine Zone</title>
    <link rel="shortcut icon" href="TemplateData/favicon.ico">
    <link rel="stylesheet" href="TemplateData/style.css">
  </head>
  <body>
    <div id="unity-container" class="unity-desktop">
      <canvas id="unity-canvas"></canvas>
      <div id="unity-loading-bar">
        <div id="unity-logo"></div>
        <div id="unity-progress-bar-empty">
          <div id="unity-progress-bar-full"></div>
        </div>
      </div>
      <div id="unity-mobile-warning">
        WebGL builds are not supported on mobile devices.
      </div>
      <div id="unity-footer">
        <div id="unity-webgl-logo"></div>
        <div id="unity-fullscreen-button"></div>
        <div id="unity-build-title">Quarantine Zone</div>
      </div>
    </div>
    <script>
      var buildUrl = "Build";
      var loaderUrl = buildUrl + "/Build.loader.js";
      var config = {
        dataUrl: buildUrl + "/Build.data.br",
        frameworkUrl: buildUrl + "/Build.framework.js.br",
        codeUrl: buildUrl + "/Build.wasm.br",
        streamingAssetsUrl: "StreamingAssets",
        companyName: "PBCJ_1QS_2021",
        productName: "Quarantine Zone",
        productVersion: "1.1",
      };

      var container = document.querySelector("#unity-container");
      var canvas = document.querySelector("#unity-canvas");
      var loadingBar = document.querySelector("#unity-loading-bar");
      var progressBarFull = document.querySelector("#unity-progress-bar-full");
      var fullscreenButton = document.querySelector("#unity-fullscreen-button");
      var mobileWarning = document.querySelector("#unity-mobile-warning");

      if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
        container.className = "unity-mobile";
        config.devicePixelRatio = 1;
        mobileWarning.style.display = "block";
        setTimeout(() => {
          mobileWarning.style.display = "none";
        }, 5000);
      } else {
        canvas.style.width = "1280px";
        canvas.style.height = "720px";
      }
      loadingBar.style.display = "block";

      var script = document.createElement("script");
      script.src = loaderUrl;
      script.onload = () => {
        createUnityInstance(canvas, config, (progress) => {
          progressBarFull.style.width = 100 * progress + "%";
        }).then((unityInstance) => {
          loadingBar.style.display = "none";
          fullscreenButton.onclick = () => {
            unityInstance.SetFullscreen(1);
          };
        }).catch((message) => {
          alert(message);
        });
      };
      document.body.appendChild(script);
    </script>
  </body>
</html>

## **Banner:**
![image](QZ_POSTER.jpeg)


## **Link YouTube:**

### Gameplay 1

[![](http://img.youtube.com/vi/zp9GMUkbVlY/0.jpg)](http://www.youtube.com/watch?v=zp9GMUkbVlY "")

### Gameplay 2

[![](http://img.youtube.com/vi/TT4wgaOrorA/0.jpg)](http://www.youtube.com/watch?v=TT4wgaOrorA "")


## DESCRIÇÃO DE ATIVIDADES

### Gilmar 

- Mapa do jogo. 
- Collider de objetos.
- Iluminação do mapa do jogo.
- Level design.
- Programação do jogo.


### Ivan

- Design do Som.
- Folder para o Jogo.
- Menu do jogo e cena de Créditos.
- Animações dos objetos.
- Programação do jogo.

### Lucas

- Game design.
- Animações dos player.
- Animações dos inimigos.
- Lógica do jogo.
- Programação do jogo. 
