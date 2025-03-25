<script setup>
import { defineEmits, defineProps } from 'vue';

const emit = defineEmits(['close']);

const props = defineProps({
    mode: String,
    evaluationResult: Object
})

function closeModal() {
    emit('close');
}

</script>
<template>
    <div class="modal">
        <div class="modal-content">
            <span class="close-btn" @click="closeModal">&times;</span>

            <template v-if="props.mode === 'submit'">
                <h2>Sveikiname! Sėkmingai įvykdėte užduotį!</h2>
                <p>Atsiliepimas:</p>
                <li>{{props.evaluationResult.feedback}}</li>
                <p style="font-size: 18px; margin-top: 5px;">Galutinis rezultatas:</p>
                <div class="progress-container">
                    <div class="progress-bar"></div>
                    <span class="percentage">{{props.evaluationResult.score}}%</span>
                </div>
            </template>
            
            <template v-if="props.mode === 'best-solution'">
                <h2>Geriausias sprendimas</h2>
                <p style="font-size: 18px; margin-top: 5px;">Rezultatas:</p>
                <div class="progress-container">
                    <div class="progress-bar"></div>
                    <span class="percentage">100%</span>
                </div>
                <button>Atsisiųsti</button>
            </template>
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

button {
    border-radius: 25px;
    background-color: #1c1c1c;
    color: #916ad5;
    padding: 5px;
    cursor: pointer;
    font-size: 20px;
    padding-left: 15px;
    padding-right: 15px;
    margin-top: 15px;
    display: block;
    margin-left: auto;
    margin-right: auto;
}

</style>
