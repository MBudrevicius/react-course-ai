import { getRequestWithAuth } from './APIRequest';

const LESSONS_API_BASE_URL = '/api/lessons';

export const getLessonsTitles = async () =>
    await getRequestWithAuth(`${LESSONS_API_BASE_URL}/titles`);

export const getLessonById = async (id) =>
    await getRequestWithAuth(`${LESSONS_API_BASE_URL}/${id}`);
