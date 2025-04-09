<script setup>
import { ref } from 'vue';
import Navbar from '../components/Navbar.vue';
import { registerUser } from '../api/user';
import router from '@/router';
import SpinningLoader from '@/components/SpinningLoader.vue';
const passwordFieldType = ref('password');
const errorMessageStatus = ref(false);
const errorMessage = ref('');
const loading = ref(false);

function toggleShow() {
    passwordFieldType.value = passwordFieldType.value === 'password' ? 'text' : 'password';
}

const formData = ref({
    email: '',
    username: '',
    password: '',
})

async function register() {
    try{
        loading.value = true;
        const result = await registerUser(formData.value);
        console.log(result.message);
        if(result.message === "Registration successful")
        {
            router.push({ name: 'home' });
            console.log("Success", result.respose.data);
        }
    } catch(error){
        console.log("Error", error);
        errorMessageStatus.value = true;
        errorMessage.value = error;
        if(typeof error ==='object'){
            errorMessage.value = "Slaptažodis turi būti bent 6 simbolių ilgio."
        } 
    } finally {
        loading.value = false;
    }
}

</script>

<template>
    <Navbar />
    <SpinningLoader v-if="loading" />
        <div class="main">
            <div class="form">
                <h1>Susikurkite paskyrą ir pradėkite mokytis React!</h1>
                <div v-if="errorMessageStatus" class="error-message">
                        <p style="color: red;">{{ errorMessage }}</p>
                </div>
                <form @submit.prevent="register">
                    <label for="username">Elektroninis paštas</label>
                    <input type="email" id="email" name="email" v-model="formData.email" required>
                    <label for="username">Prisijungimo vardas</label>
                    <input type="text" id="username" name="username" v-model="formData.username" minlength="3" maxlength="20" required>
                    <div display="grid">
                        <label for="password">Slaptažodis</label>
                    </div>
                    
                    <div style="display: ruby">
                        <span>
                        <input style="color:black" :type="passwordFieldType" id="password" name="password" v-model="formData.password" required>
                            <!-- <img v-else-if="document.getElementById('password').getAttribute('type') === 'text'" src="/svg/show.svg" class="showPassword" @click="toggleShow"> -->
                            <img v-if="passwordFieldType === 'password'" src="/svg/hide.svg" class="showPassword" @click="toggleShow">
                            <img v-else src="/svg/show.svg" class="showPassword" @click="toggleShow">
                        </span>
                    </div>
                    <div class="rounded-rectangle">
                        <button type="submit" class="submit">Registruotis</button>
                    </div>
                </form>
                <p>Jau turite paskyrą?&nbsp;<a href="/login">Prisijunkite</a></p>
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
    height: 40vh;
}

.image {
    position: fixed;
    bottom: -170px;
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