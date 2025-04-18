import axios from 'axios';
import { getCookie } from './user';

export async function sendMessage(data) {
    const token = getCookie('AuthToken');
    try {
        const response = await axios.post('http://localhost:5255/api/ai/chat', data,  {
            headers: {
                Authorization: `Bearer ${token}`
            },
            withCredentials: true
        });
        console.log("response", response);
        return response.data;
    } catch(error) {
        if (error.response) {
            console.log("Error status:", error.response.status);
            console.log("Server response:", error.response.data);
        } else {
            console.log("Error:", error.message);
        }
        throw error;
    }
}