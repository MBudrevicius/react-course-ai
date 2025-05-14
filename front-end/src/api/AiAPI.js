import { postRequestWithAuth } from './APIRequest';

const AI_API_BASE_URL = '/api/ai';

export const sendAIMessage = async (data) =>
    await postRequestWithAuth(`${AI_API_BASE_URL}/chat`, data);

export const sendAIAudio = async (data) =>
    await postRequestWithAuth(`${AI_API_BASE_URL}/transcribe`, data, 'multipart/form-data');

export const getEvaluation = async (id, data) =>
    await postRequestWithAuth(`${AI_API_BASE_URL}/evaluate/${id}`, data);