import axios from "axios";
import { getCookie } from './user';
import router from '@/router'; 

export async function getEvaluation(id, data) {
    const token = getCookie('AuthToken');
    try {
        if (!token) {
            console.log("No token found");
            router.push({ name: 'login' });
            return null;
        }else{
            const response = await axios.post(`http://localhost:5255/api/ai/evaluate/${id}`, data, {
                headers: {
                    Authorization: `Bearer ${token}`
                },
                withCredentials: true
            });
            console.log("response", response);
            return response.data;
        }
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