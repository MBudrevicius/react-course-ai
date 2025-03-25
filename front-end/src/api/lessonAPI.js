import axios from 'axios';
import { getCookie } from './user';

const LESSONS_API_BASE_URL = 'http://localhost:5255/api/lessons';

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

const PROBLEMS_API_BASE_URL = 'http://localhost:5255/api/problems';

async function fetchFromProblemsAPI(endpoint) {
    try {
        const token = getCookie('AuthToken');
        const response = await axios.get(`${PROBLEMS_API_BASE_URL}/${endpoint}`, {
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

const SUBMISSIONS_API_BASE_URL = 'http://localhost:5255/api/problems/bestSubmission';

async function fetchFromSubmissionsAPI(endpoint) {
    try {
        const token = getCookie('AuthToken');
        const response = await axios.get(`${SUBMISSIONS_API_BASE_URL}`, {
            headers: {
                Authorization: `Bearer ${token}`
            },
            withCredentials: true,
            params: {
                problemId: endpoint
            }
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
export const getTasksByLessonId = (id) => fetchFromProblemsAPI(`${id}`);
export const getBestSubmissionByProblemId = (id) => fetchFromSubmissionsAPI(id);
