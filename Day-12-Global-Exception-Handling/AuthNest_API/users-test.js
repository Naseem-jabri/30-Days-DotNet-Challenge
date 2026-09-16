import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 20,
    duration: '30s',
    insecureSkipTLSVerify: true,
};

export function setup() {

    const loginPayload = JSON.stringify({
        email: __ENV.TEST_EMAIL,
        password: __ENV.TEST_PASSWORD,
    });

    const loginResponse = http.post(
        'https://localhost:7210/api/Users/login',
        loginPayload,
        {
            headers: {
                'Content-Type': 'application/json',
            },
        }
    );

    if (loginResponse.status !== 200) {
        throw new Error(`Login failed with status ${loginResponse.status}`);
    }

    return {
        token: loginResponse.json('token'),
    };
}

export default function (data) {

    const response = http.get(
        'https://localhost:7210/api/Users',
        {
            headers: {
                Authorization: `Bearer ${data.token}`,
            },
        }
    );

    check(response, {
        'users status is 200': (r) => r.status === 200,
    });

    sleep(1);
}