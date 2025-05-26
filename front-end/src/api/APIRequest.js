import axios from 'axios';
import router from '@/router';

export function getCookie(name) {
    let match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

export async function getRequestWithAuth(endpoint) {
    const token = getCookie('AuthToken');
    if (!token) {
        router.push({ name: 'login' });
        return null;
    }

    const headers = {
        Authorization: `Bearer ${token}`,
    };

    try {
        const response = await axios.get(endpoint, {
            headers,
            withCredentials: true,
        });
        return response.data;
    } catch (error) {
        logAPIError(error);
        throw error;
    }
}

export async function postRequestWithAuth(endpoint, data, contentType = null) {
    const token = getCookie('AuthToken');
    if (!token) {
        router.push({ name: 'login' });
        return null;
    }

    const headers = {
        Authorization: `Bearer ${token}`,
    };
    if (contentType) {
        headers['Content-Type'] = contentType;
    }

    try {
        const response = await axios.post(endpoint, data, {
            headers,
            withCredentials: true,
        });
        return response.data;
    } catch (error) {
        logAPIError(error);
        throw error;
    }
}

export function logAPIError(error) {
    if (error.response) {
        console.log("Failed request to: '" + endpoint +
                "'\nRequest status: ", error.response.status +
                "\nRequest response: ", error.response.data);
    } else {
        console.log("Failed request to: '" + endpoint +
                "'\nError: ", error.message);
    }
}