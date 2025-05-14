<script setup>
import Navbar from '@/components/Navbar.vue';
import { updatePremium } from '@/api/userAPI';
import { useRouter } from 'vue-router';

const router = useRouter();

async function updatePremiumAction() {
    try {
        await updatePremium();
        router.push({ name: 'lessons' });
    }
    catch (error) {}
}
</script>
<template>
    <Navbar />
    <div class="main-container">
        <div class="content-wrapper">
            <div class="title">
                <h1 class="text-3xl font-bold">Pirkite</h1>
            </div>
            <p class="explanation">Sumokėję vienkartinį mokestį, turėsite prieigą prie visų pamokų ir užduočių!</p>
            <div class="purchase-wrapper">
                <div class="purchase-container">
                    <p class="one-time">Vienkartinis mokestis</p>
                    <span class="line"></span>
                    <h2>Kompanionas</h2>
                    <p class="price">10€</p>
                    <button class="purchase-button" @click="updatePremiumAction">Pirkti</button>
                </div>
            </div>
        </div>
        <div class="penguin-container">
            <img src="/images/up.png" alt="waving penguin" class="penguin-image">
        </div>
    </div>
</template>
<style scoped>

.main-container {
    display: flex;
    justify-content: center;
    align-items: flex-start;
    padding: 20px;
    position: relative;
    overflow: hidden;
    height: calc(100vh - 60px);
}

.content-wrapper {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
    max-width: 1200px;
    position: relative;
    z-index: 1;
}

.title {
    text-align: center;
    margin-top: 20px;
    margin-bottom: 10px;
    color: white;
}

.explanation {
    text-align: center;
    margin-bottom: 30px;
    font-size: 20px;
    color: white;
    max-width: 800px;
}

.purchase-wrapper {
    display: flex;
    justify-content: center;
    width: 100%;
    margin-bottom: 30px;
}

.purchase-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    box-shadow: #000000 0px 0px 4px 0px;
    width: 100%;
    max-width: 500px;
    padding: 30px 20px;
    background-color: #2D2D2D;
    border-radius: 12px;
}

h2 {
    font-size: 24px;
    margin-bottom: 15px;
    color: white;
}

.one-time {
    font-size: 20px;
    margin-bottom: 10px;
    color: #916ad5;
}

.line {
    width: 100%;
    height: 2px;
    background-color: #916ad5;
    margin-bottom: 20px;
}

.price {
    font-size: 24px;
    margin-bottom: 20px;
    color: white;
}

.purchase-button {
    background-color: #916ad5;
    color: white;
    border: none;
    padding: 12px 30px;
    font-size: 16px;
    cursor: pointer;
    border-radius: 5px;
    transition: background-color 0.3s ease;
}

.purchase-button:hover {
    background-color: #7c5ab8;
}

.penguin-container {
    position: absolute;
    bottom: -400px; 
    left: 50%;
    transform: translateX(-50%);
    z-index: 0;
}

.penguin-image {
    height: 900px;
    width: auto;
}

</style>