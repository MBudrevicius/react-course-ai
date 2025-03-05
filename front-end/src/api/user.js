import axios from 'axios';
import * as jwtDecode from 'jwt-decode';

export async function registerUser(data){
    try{
        const response = await axios.post('http://localhost:5255/auth/register', data, { withCredentials: true });
        return response.data;
    } catch(error){
        if (error.response) {
            console.log("Error status:", error.response.status);
            console.log("Server response:", error.response.data);
          } else {
            console.log("Error:", error.message);
          }    }
}


export async function loginUser(data){
    try{
        const response = await axios.post('http://localhost:5255/auth/login', data, { withCredentials: true });
        return response.data;
    } catch(error){
        if (error.response) {
            console.log("Error status:", error.response.status);
            console.log("Server response:", error.response.data);
          } else {
            console.log("Error:", error.message);
          }    }
}

export function getCookie(name) {
  let match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
  return match ? match[2] : null;
}

export function isUserLoggedIn() {
  const cookieValue = getCookie('AuthToken');
  if (!cookieValue) return false;
  return true;
}