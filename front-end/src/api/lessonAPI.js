import axios from 'axios';
import { getCookie } from './user';

const LESSONS_API_BASE_URL = 'http://localhost:5255/lessons';

async function fetchFromLessonsAPI(endpoint) {
    try {
        const token = getCookie('AuthToken');
        const response = await axios.get(`${LESSONS_API_BASE_URL}/${endpoint}`, {
            headers: {
                Authorization: `Bearer ${token}`
            },
            withCredentials: true
        });
        return response.data;
    } catch (error) {
        handleAPIError(error);
        throw error;
    }
}

function handleAPIError(error) {
    if (error.response) {
        console.error("Error status:", error.response.status);
        console.error("Server response:", error.response.data);
    } else {
        console.error("Error:", error.message);
    }
}

export const getLessonsTitles = () => fetchFromLessonsAPI('titles');
export const getLessonById = (id) => fetchFromLessonsAPI(id);
export const getTasksByLessonId = (id) => fetchFromLessonsAPI(`${id}/tasks`);