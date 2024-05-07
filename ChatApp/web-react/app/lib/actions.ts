"use server";

import { auth } from "@clerk/nextjs/server";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";
import useSWR, { useSWRConfig } from "swr";
import { z } from "zod";

const CreateDiscussion = z.object({
    name: z.string(),
});

export async function createDiscussion(formData: FormData) {
    // const { mutate } = useSWRConfig();

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

    // mutate("/api/my-discussions");

    // console.log(name);
    // console.log(auth().sessionClaims?.userId);
}
