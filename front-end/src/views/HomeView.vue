<script setup>
import Navbar from '../components/Navbar.vue'
import Sidebar from '../components/Sidebar.vue'
import { onMounted } from 'vue'
</script>

<template>
    <Navbar />  
    <div class="hero-rectangle">
        <div class="hero">
            <div class="grid-container">
                <div>
                    <h1 class="hero-text">Per sunkiai įkandamas riešutėlis? Tau padės</h1>
                    <h6 class="hero-text">KOMPONIONAS</h6>
                    <h1 class="hero-text">React mokymosi asistentas</h1>
                </div>
                <div style="display: flex; align-items: center;">
                    <img src="/images/wawe.png" alt="penguin image">
                </div>
            </div>
        </div>
    </div>

    <div class="section-rectangle">
        <h6 class="section" id="about">Kas tai?</h6>
    </div>

    <div class="section-content">
        <h1 style="float:left">Šis pradedantiesiems skirtas&nbsp;<span>programavimo kursas&nbsp;</span>supažindins jus su bene populiariausia svetainių kūrimo biblioteka.</h1>
    </div>

    <div class="section-rectangle">
        <h6 class="section" id="contacts">Apie mus</h6>
    </div>

    <div class="section-content">
        <h1 style="float:left"><span>React</span> – tai populiari <span>JavaScript</span> biblioteka, skirta kurti vartotojo sąsajas. Ji leidžia kūrėjams kurti <span>greitas</span>, <span>interaktyvias</span> ir <span>dinamiškas</span> interneto aplikacijas, kurios reaguoja į vartotojo veiksmus be puslapio perkrovimo.</h1>
        <h1 style="font-size: 20px; margin-top: 200px">Šią pastraipą sugeneravo <span>ChatGPT</span>, vienas iš dirbtinio intelekto modelių. Tačiau jis naudingas ne tik teksto generavimui - jis taip pat jums padės ir besimokant React’o! Taip, iškilus mokymosi sunkumams, visuomet galėsite prašyti pagalbos <span>ChatGPT</span>, kuris lauks jūsų klausimų šioje svetainėje.</h1>
    </div>
</template>

<style>
.hero {
    display: block;
    text-align: right;
    padding-right: 50px;
}

.hero-rectangle {
    background-color: #2D2D2D;
    margin-top: 100px;
    margin-bottom: 100px;
    /* display: inline-flexbox; */
    /* vertical-align: middle; */
    padding: 10px;
}

.section-rectangle {
    border-radius: 100px;
    background: #2D2D2D;
    margin-top: 200px;
    max-width: 500px;
    text-align: center;
    align-content: center;
    margin-left: auto;
    margin-right: auto;
    padding: 20px;
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
    margin-top: 40px;
    margin-left: 200px;
    margin-right: 200px;
    text-align: justify;
    float: left;
    margin-bottom: 100px;
    color: white;
}

.grid-container {
    display: grid;
    grid-template-columns: auto 250px;
}

img {
    height: 230px;
    width: auto;
}
</style>