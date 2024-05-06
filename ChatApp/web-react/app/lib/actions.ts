"use server";

import { auth } from "@clerk/nextjs/server";
import { z } from "zod";

const CreateDiscussion = z.object({
    name: z.string(),
});

export async function createDiscussion(formData: FormData) {
    const { name } = CreateDiscussion.parse({
        name: formData.get("name"),
    });

    const response = await fetch(
        "http://localhost:8080/discussions/create-discussion",
        {
            method: "POST",
            headers: {
                Authorization: `Bearer ${await auth().getToken()}`,
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                userId: auth().sessionClaims?.userId,
                name: name,
            }),
        }
    );

    const result = await response.json();

    console.log(result);

    // console.log(name);
    // console.log(auth().sessionClaims?.userId);
}
