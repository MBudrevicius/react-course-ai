<script setup>
import { ref, onMounted } from 'vue';


</script>

<template>
    <label for="files">Įkelk failus čia:</label>
    <div>
        <input type="file" id="files" name="files"/>
        <button type="submit">Pateikti</button>
    </div>
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
    width: 150px;
    align-self: right;
}
</style>