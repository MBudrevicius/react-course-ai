import axios from 'axios';
import { logAPIError, getCookie } from './APIRequest';

export function isUserLoggedIn() {
    const cookieValue = getCookie('AuthToken');
    return !!cookieValue;
}

export async function registerUser(data) {
    try {
        const response = await axios.post('/api/auth/register', data, { withCredentials: true });
        return response.data;
    } catch(error) {
        logAPIError(error);
        throw error;  
    }
}


export async function loginUser(data) {
    try {
        const response = await axios.post('/api/auth/login', data, { withCredentials: true });
        return response.data;
    } catch(error) {
        logAPIError(error);
        throw error;
    }
}