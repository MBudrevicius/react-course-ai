<script setup>
import { onMounted, ref, watch } from 'vue';

const props = defineProps({
    errorMessage: String,
    duration:{
        type: Number,
        default: 3000
    },
    isSuccess:{
        type: Boolean,
        default: false
    }
});

const emit = defineEmits(['close']);
const visible = ref(true);

function closeNotification(){
    visible.value = false;
    emit('close');
}

onMounted(() => {
    setTimeout(() => {
        closeNotification();
    }, props.duration);
});
</script>

<template>
    <div v-if="visible" :class="['notification-container', props.isSuccess ? 'success' : 'error']">
        <div class="header">{{ isSuccess ? 'Sėkmė!' : 'Klaida!' }}</div>
        <button class="close" @click="closeNotification">X</button>
        <div class="message">{{ props.errorMessage }}</div>
    </div>
</template>

<style scoped>
.notification-container{
    position: fixed;
    top: 10px;
    left: 50%;
    transform: translateX(-50%);  
    color: white;
    padding: 20px;
    border-radius: 5px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    z-index: 1000;
    transition: opacity 0.3s ease-in-out;
    opacity: 1;
    animation: fadeIn 0.5s ease-in-out;
    animation-fill-mode: forwards;
}

.notification-container.success {
    background-color: #4CAF50;
}

.notification-container.error {
    background-color: #f44336;
}

.header {
    font-size: 20px;
    font-weight: bold;
    margin-bottom: 10px;
}

.message{
    font-size: 16px;
}

.close {
    background: none;
    border: none;
    color: white;
    font-size: 20px;
    cursor: pointer;
    position: absolute;
    top: 20px;
    right: 10px;
}
.close:hover{
    color: #2D2D2D;
}

</style>