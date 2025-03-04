import axios from 'axios';

export async function registerUser(data){
    try{
        const response = await axios.post('http://localhost:5255/auth/register', data, { withCredentials: true });
        return response.data;
    } catch(error){
        throw error;
    }
}
