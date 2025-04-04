<script setup>
import { ref, onMounted } from 'vue';
import { isUserLoggedIn } from '@/api/user';

const loggedIn = ref(false);

onMounted(async () => {
  try {
    const result = isUserLoggedIn();
    loggedIn.value = result;
    console.log(result);
    console.log('User is logged in:', loggedIn.value);
  } catch (error) {
    console.log('Error checking user login status:', error);
    loggedIn.value = false;
  }
});

async function logout(){
    try{
        document.cookie = 'AuthToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC;';
        loggedIn.value = false;
        console.log('User logged out');
    } catch(error){
        console.log('Error logging out:', error);
    }
}
</script>

<template>
    <nav class="navbar">
        <div class="flex flex-wrap items-center justify-between mx-auto p-4">
            <a href="/">
                <span class="self-center text-2xl font-semibold whitespace-nowrap dark:text-white" style="color: white;">KOMPONIONAS</span>
            </a>
            <button data-collapse-toggle="navbar-default" type="button" class="inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600" aria-controls="navbar-default" aria-expanded="false">
                <span class="sr-only">Meniu</span>
                <svg class="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 17 14">
                    <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M1 1h15M1 7h15M1 13h15"/>
                </svg>
            </button>
            <div class="hidden w-full md:block md:w-auto" id="navbar-default">
            <ul class="navbar-elements">
                <li>
                <a href="/#about" class="" aria-current="page">Kas tai?</a>
                </li>
                <li>
                <a href="/#contacts" class="">Apie mus</a>
                </li>
                <li>
                    <a v-if="loggedIn" href="/lessons">Pamokos</a>
                </li>
                <div class="rounded-rectangle">
                    <li>
                        <a v-if="loggedIn" @click="logout" href="/">Atsijungti</a>
                        <a v-else href="/login">Prisijunk</a> 
                    </li>
                </div>
            </ul>
            </div>
        </div>
    </nav>
</template>

<style scoped>
.navbar {
    background-color: #2D2D2D;
    font-size: 28px;
    box-shadow: #000000 0px 0px 10px 0px;
}
  
.navbar-elements {
    display: flex;
    float: right;
}
  
a {
    color:white;
    display: block;
    font-size: 20px;
    margin-left: 20px;
    margin-right: 20px;
}
  
.rounded-rectangle {
    border-radius: 20px;
    background: #4E4E4E;
}
</style>