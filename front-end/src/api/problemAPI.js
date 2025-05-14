import { getRequestWithAuth } from './APIRequest';

const PROBLEMS_API_BASE_URL = 'http://localhost:5255/api/problems';

export const getTasksByLessonId = async (id) =>
    await getRequestWithAuth(`${PROBLEMS_API_BASE_URL}/${id}`);

export const getBestSubmissionByProblemId = async (id) =>
    await fetchFromProblemsAPI(`${PROBLEMS_API_BASE_URL}/bestSubmission/${id}`);
