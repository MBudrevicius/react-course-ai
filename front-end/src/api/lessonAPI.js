import axios from 'axios';
import { getCookie } from './user';

export async function getLessonsTitles() {
    try {
        const token = getCookie('AuthToken');
        console.log("token:",token);
        const response = await axios.get('http://localhost:5255/lessons/titles', {
            headers: {
                Authorization: `Bearer ${token}`
            },
            withCredentials: true
        });
        return response.data;
    } catch (error) {
        if (error.response) {
            console.log("Error status:", error.response.status);
            console.log("Server response:", error.response.data);
        } else {
            console.log("Error:", error.message);
        }
        throw error;
    }
}