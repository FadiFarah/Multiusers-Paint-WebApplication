
window.onInitialized = function () {

    var canvas = document.querySelector('canvas');
    if (canvas != null) {

        var context = canvas.getContext('2d');
        var cw = canvas.width;
        var ch = canvas.height;


        var paintToolBox = document.querySelector('.paint-tool-box');
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight - (window.innerHeight / 4);
        paintToolBox.width = window.innerWidth;
        paintToolBox.height = (window.innerHeight / 4);
        reOffset();

        window.onscroll = function (e) { reOffset(); }
        canvas.onresize = function (e) { reOffset(); }

        //elements
        var eraser = document.querySelector('#eraser');
        var color = document.querySelector('#color');
        var size = document.querySelector('#size');
        var clear = document.querySelector('#clear');
        var rectangle = document.querySelector('#rectangle');
        var circle = document.querySelector('#circle');
        var arrow = document.querySelector('#arrow');
        var triangle = document.querySelector('#triangle');
        var dragger = document.querySelector('#dragger');
        var filler = document.querySelector('#filler');


        //variables
        let isHolding = false;
        let paint_width = 2;
        let paint_color = "black";
        var redo_array = [];
        // save relevant information about shapes drawn on the canvas
        var rectangleShape = false;
        var circleShape = false;
        var triangleShape = false;
        var arrowShape = false;
        var shapes = [];
        var shapeDrawn = {};
        var offsetX, offsetY;
        var isDragging = false;
        var fillingPress = false;
        var draggingPress = false;
        var eraserPress = false;
        var startX, startY;
        // hold the index of the shape being dragged (if any)
        var selectedShapeIndex;


        function reOffset() {
            cw = canvas.width;
            ch = canvas.height;
            var BB = canvas.getBoundingClientRect();
            offsetX = BB.left;
            offsetY = BB.top;
        }


        function isMouseInShape(mx, my, shape) {
            if (shape.radius) {
                // this is a circle
                var dx = mx - shape.x;
                var dy = my - shape.y;
                // math test to see if mouse is inside circle
                if (dx * dx + dy * dy < shape.radius * shape.radius) {
                    // yes, mouse is inside this circle
                    return (true);
                }
            } else if (shape.width) {
                // this is a rectangle
                var rLeft = shape.x;
                var rRight = shape.x + shape.width;
                var rTop = shape.y;
                var rBott = shape.y + shape.height;
                // math test to see if mouse is inside rectangle
                //if rectangle was drawn from left to right
                if (rLeft < rRight) {
                    if ((mx > rLeft && mx < rRight) && (my > rTop && my < rBott)) {
                        return (true);
                    }
                }
                //if rectangle was drawn from right to left
                else {
                    if ((mx < rLeft && mx > rRight) && (my > rTop && my < rBott)) {
                        return (true);
                    }
                }

            }
            else if (shape.lengthX) {
                // this is a rectangle
                var rLeft = shape.x;
                var rRight = shape.x + shape.lengthX;
                var rTop = shape.y;
                var rBott = shape.y + shape.lengthY;
                // math test to see if mouse is inside rectangle
                //if rectangle was drawn from left to right

                if ((mx > rLeft && mx < rRight) && (my > rTop && my < rBott)) {
                    return (true);
                }
                else if ((mx > rLeft && mx < rRight) && (my < rTop && my > rBott)) {
                    return (true);
                }
            }
            // the mouse isn't in any of the shapes
            return (false);
        }


        function handleMouseDown(e) {
            redo_array = [];
            isHolding = true;

            startX = parseInt(e.clientX - offsetX);
            startY = parseInt(e.clientY - offsetY);
            // test mouse position against all shapes
            // post result if mouse is in a shape
            for (var i = 0; i < shapes.length; i++) {
                if (isMouseInShape(startX, startY, shapes[i])) {
                    // the mouse is inside this shape
                    // select this shape
                    // set the isDragging flag
                    if (draggingPress) {
                        isDragging = true;
                        selectedShapeIndex = i;

                    }
                    else if (fillingPress) {
                        shapes[i].fillcolor = paint_color;
                        drawAll();
                    }
                    else if (eraserPress) {
                        shapeDrawn = {};
                        shapes.splice(i, 1)
                        drawAll();

                    }
                    // and return (==stop looking for 
                    //     further shapes under the mouse)
                    return;
                }
            }

        }

        function handleMouseUp(e) {
            if (!isHolding) { return; }
            isHolding = false;
            if (isDragging == false && eraserPress == false && fillingPress == false) {
                shapes.push(shapeDrawn);
            }
            else {
                // the drag is over -- clear the isDragging flag
                isDragging = false;
            }
        }


        function drawShape(e) {
            if (isHolding == true && isDragging == false && fillingPress == false && eraserPress == false) {
                drawAll();
                var mouseX = parseInt(e.clientX - offsetX);
                var mouseY = parseInt(e.clientY - offsetY);
                // how far has the mouse dragged from its previous mousemove position?
                var dx = mouseX - startX;
                var dy = mouseY - startY;
                context.fillStyle = paint_color;
                if (rectangleShape == true) {
                    shapeDrawn = { x: startX, y: startY, width: dx, height: dy, fillcolor: paint_color }
                    context.fillRect(startX, startY, dx, dy);
                }
                else if (circleShape == true) {
                    shapeDrawn = { x: startX, y: startY, radius: Math.abs(dx / 2), fillcolor: paint_color }
                    context.beginPath();
                    context.arc(startX, startY, Math.abs(dx / 2), 0, Math.PI * 2);
                    context.fill();
                    context.closePath();
                }
                else if (triangleShape == true) {
                    shapeDrawn = { x: startX, y: startY, lengthX: dx, lengthY: dy, fillcolor: paint_color };
                    context.beginPath();
                    context.moveTo(startX, startY);
                    context.lineTo(Math.abs(dx + startX), startY);
                    context.lineTo(Math.abs(dx / 2) + startX, Math.abs(dy + startY));
                    context.fill();
                    context.closePath();
                }
                else if (arrowShape == true) {
                    var headlen = 20; // length of head in pixels
                    var angle = Math.atan2(dy, dx);
                    shapeDrawn = { x: startX, y: startY, mx: mouseX, my: mouseY, headlen: headlen, angle: angle, fillcolor: paint_color, linewidth: paint_width };
                    context.beginPath();
                    context.strokeStyle = paint_color;
                    context.moveTo(startX, startY);
                    context.lineTo(mouseX, mouseY);
                    context.lineTo(mouseX - headlen * Math.cos(angle - Math.PI / 6), mouseY - headlen * Math.sin(angle - Math.PI / 6));
                    context.moveTo(mouseX, mouseY);
                    context.lineTo(mouseX - headlen * Math.cos(angle + Math.PI / 6), mouseY - headlen * Math.sin(angle + Math.PI / 6));
                    context.lineWidth = "3";
                    context.stroke();
                    context.closePath();
                }
            }
            else if (isHolding == true && isDragging == true) {
                // calculate the current mouse position         
                var mouseX = parseInt(e.clientX - offsetX);
                var mouseY = parseInt(e.clientY - offsetY);
                // how far has the mouse dragged from its previous mousemove position?
                var dx = mouseX - startX;
                var dy = mouseY - startY;
                // move the selected shape by the drag distance
                var selectedShape = shapes[selectedShapeIndex];
                selectedShape.x += dx;
                selectedShape.y += dy;
                // clear the canvas and redraw all shapes
                drawAll();
                // update the starting drag position (== the current mouse position)
                startX = mouseX;
                startY = mouseY;
            }
        }

        function drawAll() {
            context.clearRect(0, 0, cw, ch);
            for (var i = 0; i < shapes.length; i++) {
                var shape = {};
                shape = shapes[i];
                if (shape.radius) {
                    // it's a circle
                    context.beginPath();
                    context.arc(shape.x, shape.y, shape.radius, 0, Math.PI * 2);
                    context.fillStyle = shape.fillcolor;
                    context.fill();
                    context.fillStyle = "none";
                    context.closePath();
                } else if (shape.width) {
                    // it's a rectangle
                    context.fillStyle = shape.fillcolor;
                    context.fillRect(shape.x, shape.y, shape.width, shape.height);
                    context.fillStyle = "none";
                }
                else if (shape.lengthX) {
                    context.beginPath();
                    context.fillStyle = shape.fillcolor;
                    context.moveTo(shape.x, shape.y);
                    context.lineTo(shape.lengthX + shape.x, shape.y);
                    context.lineTo(Math.abs(shape.lengthX / 2) + shape.x, Math.abs(shape.lengthY + shape.y));
                    context.fill();
                    context.fillStyle = "none";
                    context.closePath();
                }
                else if (shape.angle) {
                    context.beginPath();
                    context.strokeStyle = shape.fillcolor;
                    context.lineWidth = "3";
                    context.moveTo(shape.x, shape.y);
                    context.lineTo(shape.mx, shape.my);
                    context.lineTo(shape.mx - shape.headlen * Math.cos(shape.angle - Math.PI / 6), shape.my - shape.headlen * Math.sin(shape.angle - Math.PI / 6));
                    context.moveTo(shape.mx, shape.my);
                    context.lineTo(shape.mx - shape.headlen * Math.cos(shape.angle + Math.PI / 6), shape.my - shape.headlen * Math.sin(shape.angle + Math.PI / 6));
                    context.stroke();
                    context.fillStyle = "none";
                    context.closePath();
                }
            }
        }


        


        //EventListeners
        canvas.addEventListener('mousedown', handleMouseDown);
        canvas.addEventListener('mouseup', handleMouseUp);
        canvas.addEventListener('mousemove', drawShape);
        color.addEventListener('input', (e) => paint_color = e.target.value);
        size.addEventListener('input', (e) => paint_width = e.target.value);
        clear.addEventListener('click', () => { shapes = []; redo_array = []; drawAll(); });
        rectangle.addEventListener('click', () => { rectangleShape = true; circleShape = false; arrowShape = false; triangleShape = false; draggingPress = false; fillingPress = false; eraserPress = false; });
        circle.addEventListener('click', () => { rectangleShape = false; circleShape = true; arrowShape = false; triangleShape = false; draggingPress = false; fillingPress = false; eraserPress = false; });
        arrow.addEventListener('click', () => { rectangleShape = false; circleShape = false; arrowShape = true; triangleShape = false; draggingPress = false; fillingPress = false; eraserPress = false; });
        triangle.addEventListener('click', () => { rectangleShape = false; circleShape = false; arrowShape = false; triangleShape = true; draggingPress = false; fillingPress = false; eraserPress = false; });
        dragger.addEventListener('click', () => { rectangleShape = false; circleShape = false; arrowShape = false; triangleShape = false; draggingPress = true; fillingPress = false; eraserPress = false; });
        filler.addEventListener('click', () => { rectangleShape = false; circleShape = false; arrowShape = false; triangleShape = false; draggingPress = false; fillingPress = true; eraserPress = false; });
        eraser.addEventListener('click', () => { rectangleShape = false; circleShape = false; arrowShape = false; triangleShape = false; draggingPress = false; fillingPress = false; eraserPress = true; });

        window.undoDraw = function () {
            if (shapes.length == 0) return;
            redo_array.push(shapes.pop());
            drawAll();
        }


        window.redoDraw = function () {
            if (redo_array.length == 0) return;
            shapes.push(redo_array.pop());
            drawAll();
        }

        window.getShapesList=function(){
            return shapes;
        }

        window.updateAndDrawShapes = function (returnedShapes) {
            shapes = returnedShapes;
            drawAll();
        }

        window.addEventListener('resize', () => {
            var paintToolBox = document.querySelector('.paint-tool-box');
            canvas.width = window.innerWidth;
            canvas.height = window.innerHeight - (window.innerHeight / 4);
            paintToolBox.width = window.innerWidth;
            paintToolBox.height = (window.innerHeight / 4);
            reOffset();
            drawAll();

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



