
window.onInitialized = function() {
        var canvas = document.querySelector('canvas');
        if (canvas != null) {

            var context = canvas.getContext('2d');

            //elements
            var eraser = document.querySelector('#eraser');
            var color = document.querySelector('#color');
            var size = document.querySelector('#size');
            var clear = document.querySelector('#clear');
            var undo = document.querySelector('#undo');
            var redo = document.querySelector('#redo');

            //variables
            let painting = false;
            let paint_width = 2;
            let paint_color = "black";
            var undo_array = [];
            var redo_array = [];
            var index = -1;



            function startPosition(e) {
                redo_array = [];
                painting = true;
                //we execute the paint() here so we can draw dots without actually moving.
                paint(e);
            }

            function finishedPosition() {
                painting = false;
                context.beginPath();
                undo_array.push(context.getImageData(0, 0, canvas.width, canvas.height));
                index++;
            }

            function paint(e) {
                //if we aren't holding down the mouse, it won't do anything
                if (!painting) return;
                context.lineWidth = paint_width;
                context.strokeStyle = paint_color;
                context.lineCap = 'round';
                //e.client means the currrent position of the mouse
                context.lineTo(e.clientX - (e.clientX - e.offsetX), e.clientY - (e.clientY - e.offsetY));
                context.stroke();
            }

            function undoDraw() {
                if (index <= 0) {
                    context.clearRect(0, 0, canvas.width, canvas.height);
                } else {
                    index--;
                    redo_array.push(undo_array.pop());
                    context.putImageData(undo_array[index], 0, 0);
                }
            }

            function redoDraw() {
                if (redo_array.length == 0) return;
                index++;
                context.putImageData(redo_array.pop(), 0, 0);
                undo_array.push(context.getImageData(0, 0, canvas.width, canvas.height));
            }


            //EventListeners
            canvas.addEventListener('mousedown', startPosition);
            canvas.addEventListener('mouseup', finishedPosition);
            canvas.addEventListener('mousemove', paint);
            color.addEventListener('input', (e) => paint_color = e.target.value);
            size.addEventListener('input', (e) => paint_width = e.target.value);
            eraser.addEventListener('click', () => paint_color = "white");
            clear.addEventListener('click', () => context.clearRect(0, 0, canvas.width, canvas.height));
            undo.addEventListener('click', undoDraw);
            redo.addEventListener('click', redoDraw);
    
        window.addEventListener('resize', () => {
            var paintToolBox = document.querySelector('.paint-tool-box');
            canvas.width = window.innerWidth;
            canvas.height = window.innerHeight - (window.innerHeight / 4);
            paintToolBox.width = window.innerWidth;
            paintToolBox.height = (window.innerHeight / 4);
            if (undo_array.length>0)
                context.putImageData(undo_array[undo_array.length - 1], 0, 0);
        })
    
        }

}
window.getNewImage = function () {
    var canvas = document.querySelector('canvas');
    var dataURL = canvas.toDataURL();
    return dataURL;
}
window.drawImage = function (imageURL) {
    var canvas = document.querySelector('canvas');
    var context = canvas.getContext("2d");
    var img = new Image();
    img.src = imageURL;
    img.onload = () => {
        context.clearRect(0, 0, canvas.width, canvas.height);
        context.drawImage(img, 0, 0, canvas.width, canvas.height);
    }
}



