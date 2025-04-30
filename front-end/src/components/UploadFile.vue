<script setup>
import { ref, onMounted, watch } from 'vue';
import { getTasksByLessonId, getBestSubmissionByProblemId } from '../api/lessonAPI';
import SolutionEvaluation from './SolutionEvaluation.vue';
import { getEvaluation } from '@/api/evaluationAPI';
import { useRoute } from 'vue-router'
import SpinningLoader from './SpinningLoader.vue';
import NotificationItem from './Notification.vue';

const showScoreModal = ref(false);
const modalMode = ref('');
const fileContent = ref('');
const fileInput = ref(null);
const evaluationResult = ref(null);
const hasSubmission = ref(false);
const fileError = ref(''); 
const loading = ref(false);
const showNotification = ref(false);
const isSuccessCheck = ref(false);
const route = useRoute();
const lessonId = ref(route.params.id);

watch(() => route.params.id, async () => {
    lessonId.value = route.params.id;
    await clearInput();
    await checkSubmission();
});

onMounted(async () => {
    await checkSubmission();
});

async function toggleScoreModal(mode) {
    try{
        loading.value = true;
        console.log(lessonId.value);
        if(mode == 'submit') {
            await sendFile();
        }
        modalMode.value = mode;
        showScoreModal.value = true;
    } catch(error){
        console.error('Error in toggleScoreModal:', error);
    } finally {
        loading.value = false;
    }
    
}


function closeScoreModal() {
    showScoreModal.value = false;
}

async function sendFile(){
    try{
        const tasks = await getTasksByLessonId(lessonId.value);
        const response = await getEvaluation(tasks[0].id, { codeSubmission: fileContent.value });
        evaluationResult.value = response;
    }
    catch(error){
        console.error(error);
    }
}

function readSubmission(event) {
    fileError.value = '';
    const file = event.target.files[0];

    if (file && !file.name.toLowerCase().endsWith('.js')) {
        fileError.value = 'Leidžiami tik .js tipo failai. Bandykite dar kartą.';
        showNotification.value = true;
        isSuccessCheck.value = false;
        if(fileInput.value){
            fileInput.value.value = null;
        }
        return;
    }

    const reader = new FileReader();
    reader.onload = () => {
        fileContent.value = reader.result;
        console.log("Extracted text:", fileContent.value);
    }
    reader.readAsText(file);
}

async function checkSubmission() {
    try {
        const tasks = await getTasksByLessonId(lessonId.value);
        const submission = await getBestSubmissionByProblemId(tasks[0].id)
        hasSubmission.value = true;
    } catch (error) {
        console.log('checkSubmission threw an error:', error);
        hasSubmission.value = false;
    }
}

async function clearInput(){
    fileContent.value = '';
    if(fileInput.value){
        fileInput.value.value = null;
    }
}
</script>

<template>
    <SpinningLoader v-if="loading" />
    <NotificationItem v-if="showNotification" @close="showNotification = false" :errorMessage="fileError" :isSuccess="isSuccessCheck"/>
    <label for="files">Įkelk failus čia:</label>
    <div>
        <input 
            type="file" 
            id="files" 
            name="files" 
            accept=".js"
            @change="readSubmission" 
            ref="fileInput" 
        />
        <button type="submit" @click="toggleScoreModal('submit')">Pateikti</button>
        <button v-if="hasSubmission" @click="toggleScoreModal('best-solution')">
            Geriausias sprendimas
        </button>
    </div>
    <SolutionEvaluation
        v-if="showScoreModal"
        :mode="modalMode"
        :evaluationResult="evaluationResult"
        @close="closeScoreModal"
    />
</template>

<style scoped>
label {
    font-size: 20px;
    color: #916ad5;
    margin-top: 20px;
    margin-bottom: 10px;
    text-align: left;
}

input {
    border-radius: 20px;
    width: 420px;
    margin-bottom: 20px;
    color: white;
    background-color: #4E4E4E;
}

input[type=file]::file-selector-button {
    background-color: #916ad5;
}

button {
    border-radius: 25px;
    background-color:  #4E4E4E;
    color: #916ad5;
    padding: 5px;
    cursor: pointer;
    font-size: 20px;
    margin-left: 10px;
    align-self: right;
    padding-left: 15px;
    padding-right: 15px;
}
</style>
