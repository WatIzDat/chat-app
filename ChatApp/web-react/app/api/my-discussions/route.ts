import { auth } from "@clerk/nextjs/server";

export async function GET(request: Request) {
    const response = await fetch(
        `http://localhost:8080/discussions/get-created-discussions-by-user?userId=${
            auth().sessionClaims?.userId
        }`,
        {
            method: "GET",
            headers: {
                Authorization: `Bearer ${await auth().getToken()}`,
            },
        }
    );

    const result = await response.json();

    return Response.json({ result });
}
