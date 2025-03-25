<script setup>
import { ref } from 'vue';
import SolutionEvaluation from './SolutionEvaluation.vue';
import { getEvaluation } from '@/api/evaluationAPI';
import { useRoute } from 'vue-router'

const showScoreModal = ref(false);
const modalMode = ref('');

const fileContent = ref('');
const evaluationResult = ref(null);

const route = useRoute();
const lessonId = ref(route.params.id);

async function toggleScoreModal(mode) {
    console.log(lessonId.value);
    await sendFile();
    modalMode.value = mode;
    showScoreModal.value = true;
}

function closeScoreModal() {
    showScoreModal.value = false;
}

async function sendFile(){
    try{
        const response = await getEvaluation({ problemId: lessonId.value, codeSubmission: fileContent.value });
        evaluationResult.value = response;
    }
    catch(error){
        console.error(error);
    }

}

function readSubmission(event) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onload = () => {
        fileContent.value = reader.result;
        console.log("Extracted text:", fileContent.value);
    }

    reader.readAsText(file);
}
</script>

<template>
    <label for="files">Įkelk failus čia:</label>
    <div>
        <input type="file" id="files" name="files" @change="readSubmission"/>
        <button type="submit" @click="toggleScoreModal('submit')">Pateikti</button>
        <button @click="toggleScoreModal('best-solution')">Geriausias sprendimas</button>
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
    width: 300px;
    margin-bottom: 20px;
    color: white;
    background-color: #2D2D2D;
}

input[type=file]::file-selector-button {
  background-color: #916ad5;
}

button {
    border-radius: 25px;
    background-color: #2D2D2D;
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