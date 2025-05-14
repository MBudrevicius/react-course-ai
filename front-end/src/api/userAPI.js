import { getRequestWithAuth, postRequestWithAuth } from './APIRequest';

const USER_API_BASE_URL = 'http://localhost:5255/api/user';

export const updatePremium = async () =>
    await postRequestWithAuth(`${USER_API_BASE_URL}/updatePremium`, null);

export const getSolutions = async () =>
    await getRequestWithAuth(`${USER_API_BASE_URL}/solutions`);