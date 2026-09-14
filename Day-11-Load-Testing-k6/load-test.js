import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    vus: 20,
    duration: '30s',
    insecureSkipTLSVerify: true,
};

export default function () {

    //Login
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

    check(loginResponse, {
        'login status is 200': (r) => r.status === 200,
    });

    if (loginResponse.status !== 200) {
        return;
    }

    //Get JWT token
    const token = loginResponse.json('token');

    //Access protected endpoint
    const usersResponse = http.get(
        'https://localhost:7210/api/Users',
        {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        }
    );

    check(usersResponse, {
        'users status is 200': (r) => r.status === 200,
    });

    sleep(1);
}
