<script setup>
import Navbar from '../components/Navbar.vue'

function get() {
    const audio = document.getElementById("audio");
    audio.play();
}
</script>

<template>
    <Navbar />  
    <div class="hero-rectangle" style="overflow: hidden;">
        <div class="hero">
            <div class="grid-container">
                <div style="align-self: center;">
                    <h1 class="hero-text">Per sunkiai įkandamas riešutėlis? Tau padės</h1>
                    <h6 class="hero-text">KOMPONIONAS</h6>
                    <h1 class="hero-text">React mokymosi asistentas</h1>
                </div>
                <div style="overflow: hidden;">
                    <img src="/images/wave.png" alt="waving penguin">
                </div>
            </div>
        </div>
    </div>
    
    <div style="display:grid; overflow: hidden;">
        <div class="section-rectangle">
            <h6 class="section" id="about">Kas tai?</h6>
        </div>

        <div class="section-content">
            <h1 style="float:left"><span>Komponionas</span> – tai moderni mokymosi platforma, skirta visiems, siekiantiems gilinti savo React žinias.
                Svetainė apjungia <span>kokybišką mokomąją medžiagą</span> ir <span>pažangų dirbtinio intelekto asistentą</span>, kuris realiuoju laiku atsako į jūsų klausimus, padeda spręsti problemas ir užtikrina sklandų mokymosi procesą.
                Komponionas suteikia galimybę mokytis savarankiškai, išlaikant aukštą žinių kokybę ir nuoseklų progresą. Nesvarbu, ar tik pradedate, ar siekiate tobulinti jau turimas žinias - šioje mokymosi plaftormoje rasite įvairaus lygio užduočių, kurias galėsite spręsti <span>bet kuriuo paros laiku</span>.</h1>
        </div>

        <div class="section-rectangle">
            <h6 class="section" id="react">Apie React</h6>
        </div>

        <div class="section-content">
            <h1 style="float:left"><span>React</span> – tai viena populiariausių šiuolaikinių <span>JavaScript bibliotekų</span>, skirta kurti interaktyvias vartotojo sąsajas. 
                Biblioteka leidžia programuotojams kurti <span>komponentais</span> pagrįstas aplikacijas, užtikrinant aiškesnę struktūrą, geresnį funkcionalumą ir lengvesnį kodų palaikymą. 
                React žinios – <span>viena paklausiausių kompetencijų</span> šiandienos darbo rinkoje.</h1>
        </div>
    </div>

    <div class="footer" style="overflow: hidden;">
        <div class="text">@PVP projektas</div>
        <div class="penguins">
            <img src="/images/Pijus.png">
            <button @click="get()"><img src="/images/as.png" class="hover-pointer"></button>
            <img src="/images/Jonas.png">
            <img src="/images/Martynas.png">
            <img src="/images/Matas.png">
        </div>
    </div>
    <audio id="audio" src="/src/omg.mp3"></audio>
</template>

<style scoped>
.footer {
    background-color: #2D2D2D;
    box-shadow: #000000 0px 0px 10px 0px inset;
    padding: 30px;
    display: grid;
    grid-template-areas:
    "text penguins";
    grid-template-columns: 1fr 1fr;
}

.text {
    grid-area: text;
    color: gray;
    font-size: 20px;
    text-align: justify;
    display: flex;
    justify-content: center;
    align-items: center;
}

.penguins {
    grid-area: penguins;
    display: flex;
    justify-content: center;
    align-items: center;
    border-left: #55555583 1px solid;
    gap: 20px;
}

.penguins img {
    width: auto;
    height: 100px;
}

.hover-pointer:hover {
    cursor: pointer;
}

.hero {
    text-align: right;
}

.hero-rectangle {
    background-color: #2D2D2D;
    box-shadow: #000000 0px 0px 10px 0px inset;
    margin-bottom: 100px;
}

.section-rectangle {
    border-radius: 100px;
    background: #2D2D2D;
    margin-left: auto;
    margin-right: auto;
    padding: 20px;
    width: 400px;
    text-align: center;
    box-shadow: #000000 0px 0px 4px 0px;
}

h1.hero-text {
    color: white;
    font-size: 32px;
}

span {
    color:#9f7cda;
}

h6.hero-text {
    color: #916ad5;
    font-size: 80px;
}

p {
    color: white;
    font-size: 20px;
}

.section {
    font-size: 50px;
    color: #916ad5;
}

.section-content {
    font-size: 32px;
    margin-left: 200px;
    margin-right: 200px;
    text-align: justify;
    float: left;
    color: white;
    margin-top: 50px;
    margin-bottom: 50px;
}

.grid-container {
    display: grid;
    grid-template-columns: auto 40vw;
    height: 400px;
}

.hero img {
    height: 500px;
    width: auto;
    margin-left: 100px;
    margin-top: 50px;
}

@media (max-height: 960px) {
  .hero img{
    height: 450px;
  }
}

@media (max-height: 840px) {
  .hero img{
    display: none;
  }

  .hero {
    text-align: center;
  }

  .grid-container {
    grid-template-columns: auto;
    height: auto;
    padding: 20px;
  }
}

@media (max-height: 480px) {
  .penguins img{
    height: 50px;
  }
}
</style>