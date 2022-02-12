function previewFile() {
    const content = document.querySelector('.content');
    const [file] = document.querySelector('input[type=file]').files;
    const reader = new FileReader();
  
    reader.addEventListener("load", () => {
      // this will then display a text file
      content.innerText = reader.result;
    }, false);
  
    if (file) {
      reader.readAsText(file);
    }
  }
  
  document.getElementById('file').onchange = function () {
    var file = this.files[0];
    var reader = new FileReader();
    reader.onload = function (progressEvent) {
      var fileContentArray = this.result.split(/\r\n|\n/);
      for (var line = 0; line < lines.length - 1; line++) {
        console.log(line + " --> " + lines[line]);
      }
    };
    reader.readAsText(file);
  };