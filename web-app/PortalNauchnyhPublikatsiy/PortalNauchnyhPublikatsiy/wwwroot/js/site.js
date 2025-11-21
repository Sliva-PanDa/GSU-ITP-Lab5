window.addEventListener('load', function () {
    // Находим наш холст
    const canvas = document.getElementById('cosmic-dust-canvas');
    if (!canvas) {
        console.error("Canvas element not found!");
        return;
    }

    const ctx = canvas.getContext('2d');

    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;

    let particlesArray;

    // Класс для отдельной частицы (звезды)
    class Particle {
        constructor(x, y, directionX, directionY, size, color) {
            this.x = x;
            this.y = y;
            this.directionX = directionX;
            this.directionY = directionY;
            this.size = size;
            this.color = color;
        }

        // Метод для отрисовки частицы
        draw() {
            ctx.beginPath();
            ctx.arc(this.x, this.y, this.size, 0, Math.PI * 2, false);
            ctx.fillStyle = this.color;
            ctx.fill();
        }

        // Метод для обновления положения частицы в каждом кадре
        update() {
            if (this.x > canvas.width || this.x < 0) {
                this.x = (this.x > canvas.width) ? 0 : canvas.width;
            }
            if (this.y > canvas.height || this.y < 0) {
                this.y = (this.y > canvas.height) ? 0 : canvas.height;
            }

            this.x += this.directionX;
            this.y += this.directionY;

            this.draw();
        }
    }

    // Функция для создания массива частиц
    function init() {
        particlesArray = [];
        let numberOfParticles = (canvas.height * canvas.width) / 9000;
        for (let i = 0; i < numberOfParticles; i++) {
            let size = (Math.random() * 1.5) + 0.5; 
            let x = Math.random() * innerWidth;
            let y = Math.random() * innerHeight;
            let directionX = (Math.random() * 0.4) - 0.2; 
            let directionY = (Math.random() * 0.4) - 0.2; 
            let color = 'rgba(225, 225, 230, 0.8)'; 

            particlesArray.push(new Particle(x, y, directionX, directionY, size, color));
        }
    }

    // Главный цикл анимации
    function animate() {
        requestAnimationFrame(animate);
        ctx.clearRect(0, 0, innerWidth, innerHeight);

        for (let i = 0; i < particlesArray.length; i++) {
            particlesArray[i].update();
        }
    }

    window.addEventListener('resize', function () {
        canvas.width = innerWidth;
        canvas.height = innerHeight;
        init(); 
    });

    init();
    animate();
});