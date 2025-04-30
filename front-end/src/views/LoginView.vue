<script setup>
import { ref, TrackOpTypes } from 'vue';
import Navbar from '../components/Navbar.vue';
import { loginUser } from '../api/user';
import router from '@/router';
import SpinningLoader from '@/components/SpinningLoader.vue';
import NotificationItem from '@/components/Notification.vue';

const passwordFieldType = ref('password');
const errorMessage = ref('');
const loading = ref(false);
const showNotification = ref(false);
const isSuccessCheck = ref(false);

function toggleShow() {
    passwordFieldType.value = passwordFieldType.value === 'password' ? 'text' : 'password';
}

const formData = ref({
    usernameOrEmail: '',
    password: ''
});

async function login(){
    try{
        loading.value = true;
        const result = await loginUser(formData.value);
        console.log(result.message);
        if(result.message === "Login successful"){
            isSuccessCheck.value = true;
            errorMessage.value = "Sėkmingai prisijungta";
            showNotification.value = true;
            console.log("Success", result);
            setTimeout(() => {
                router.push({ name: 'home' });
            }, 500);
        }
    } catch(error){
        console.log("Error", error);
        isSuccessCheck.value = false;
        errorMessage.value = "Neteisingas prisijungimo vardas arba slaptažodis";        
        showNotification.value = true;
    } finally {
        loading.value = false;
    }
}

</script>

<template>
    <Navbar />
    <SpinningLoader v-if="loading" />
    <NotificationItem v-if="showNotification" @close="showNotification = false" :errorMessage="errorMessage" :isSuccess="isSuccessCheck"/>
        <div class="main">
            <div class="form">
                <h1>Įveskite prisijungimo duomenis</h1>

                <form @submit.prevent="login">
                    <label for="username">Prisijungimo vardas</label>
                    <input type="text" id="username" name="username" v-model="formData.usernameOrEmail" required>
                    <div display="grid">
                        <label for="password">Slaptažodis</label>
                    </div>
                    
                    <div style="display: ruby">
                        <span>
                        <input style="color:black" :type="passwordFieldType" id="password" name="password" v-model="formData.password" required>
                            <img v-if="passwordFieldType === 'password'" src="/svg/hide.svg" class="showPassword" @click="toggleShow">
                            <img v-else src="/svg/show.svg" class="showPassword" @click="toggleShow">
                        </span>
                    </div>
                    <div class="rounded-rectangle">
                        <button type="submit" class="submit">
                            <span v-if="!loading">Prisijungti</span>
                        </button>
                    </div>
                </form>
                <p>Neturite paskyros?&nbsp;<a href="/register">Užsiregistruokite</a></p>
            </div>
            <div class="image">
                <img src="/images/happy.png" alt="happy penguin">
            </div>
        </div>
</template>

<style scoped>
main {
    display: flex;
    flex-direction: column;
}

h1 {
    margin-top: 50px;
    font-size: 35px;
    text-align: center;
    color: white
}

label, input {
    display: block;
    margin-top: 20px;
}

input {
    border-radius: 20px;
    width: 300px;
}

.form {
    text-align: center;
    display: block;
}

form
{
    display: inline-block;
    margin-left: auto;
    margin-right: auto;
    text-align: left;
}


label {
    font-size: 20px;
    margin-bottom: -15px;
    color: white;
}

.rounded-rectangle {
    margin-top: 20px;
    max-width: 250px;
    border-radius: 20px;
    background: #2D2D2D;
    text-align: center;
    align-content: center;
    margin-left: auto;
    margin-right: auto;
}
.rounded-rectangle,button:hover{
    cursor: pointer;
}

button.submit {
    font-size: 25px;
    color: white;
}

a {
    color: #916ad5;
}

a:hover {
    color: #9f7cda;
    text-decoration: underline;
}

p {
    font-size: 25px;
    margin-top: 20px;
}

.image img {
    width: auto;
    height: 30vh;
}

.image {
    position: fixed;
    bottom: 0px;
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;

}

.showPassword {
    position: absolute;
    width: 25px;
    height: 25px;
    display: inline-block;
    margin-left: -45px;
    margin-top: 28.5px;
    filter: invert(47%) sepia(72%) saturate(467%) hue-rotate(219deg) brightness(88%) contrast(89%);
}

@media (max-height: 744px) {
  .image img{
    display: none;
  }
}
</style>