<script setup>
import { defineEmits } from 'vue';

const emit = defineEmits(['close']);

function closeModal() {
    emit('close'); // Emit the close event
}
</script>

<template>
    <div class="modal">
        <div class="modal-content">
            <span class="close-btn" @click="closeModal">&times;</span>
            <h2>Sveikiname! Sėkmingai įvykdėte užduotį!</h2>
            <p>Kur galėtumėte patobulėti:</p>
            <li>Sintaksė: 100 taškų</li>
            <li>Kodo teisingumas: 100 taškų</li>
            <li>Užduoties taisyklingumas: 100 taškų</li>
            <p style="font-size: 18px; margin-top: 5px;">Galutinis rezultatas:</p>
            <div class="progress-container">
                <div class="progress-bar"></div>
                <span class="percentage">100%</span>
            </div>
        </div>
    </div>
</template>

<style scoped>
.progress-container {
    display: flex;
    align-items: center;
    gap: 10px;
}

.percentage {
    font-size: 20px;
    color: white;
    min-width: 50px;
    text-align: right;
    display: flex;
    margin-top: 10px; 
}

.progress-bar {
    align-items: center;
    background: #916ad5;
    width: 0%;
    height: 20px;
    border-radius: 20px;
    transition: 0.5s linear;
    transition-property: width, background-color;
    animation: progress 5s forwards;
    margin-top: 10px;
}

@keyframes progress {
    from {
        width: 0%;
        background-color: #eb4034;
    }
    to {
        width: 100%;
        background-color: #22992c;
    }
}

h2 {
    color: white;
    font-size: 25px;
    text-align: center;
    margin-bottom: 20px;
    border-bottom: #4E4E4E 2px solid;
}

li {
    color: white;
    font-size: 20px;
    margin-left: 40px;
}

.modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
}

.modal-content {
    background: #2D2D2D;
    padding: 20px;
    border-radius: 10px;
    width: 700px;
    position: relative;
}

.close-btn {
    position: absolute;
    top: 5px;
    right: 15px;
    font-size: 30px;
    cursor: pointer;
    color: #916ad5;
}
</style>
