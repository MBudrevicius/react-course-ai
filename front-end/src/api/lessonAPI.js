import axios from 'axios';
import { getCookie } from './user';
import router from '@/router'; 

const LESSONS_API_BASE_URL = 'http://localhost:5255/api/lessons';

async function fetchFromLessonsAPI(endpoint) {
    const token = getCookie('AuthToken');
    try {
        if (!token) {
            console.log("No token found lessons");
            router.push({ name: 'login' });
            return null;
        }else{
            const response = await axios.get(`${LESSONS_API_BASE_URL}/${endpoint}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                },
                withCredentials: true
            });
            return response.data;
        }
    } catch (error) {
        handleAPIError(error);
        throw error;
    }
}

const PROBLEMS_API_BASE_URL = 'http://localhost:5255/api/problems';

async function fetchFromProblemsAPI(endpoint) {
    const token = getCookie('AuthToken');
    try {
        if (!token) {
            console.log("No token found problems");
            router.push({ name: 'login' });
            return null;
        }else{
            const response = await axios.get(`${PROBLEMS_API_BASE_URL}/${endpoint}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                },
                withCredentials: true
            });
            return response.data;
        }
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
export const getTasksByLessonId = (id) => fetchFromProblemsAPI(`${id}`);
export const getBestSubmissionByProblemId = (id) => fetchFromProblemsAPI(`bestSubmission/${id}`);
