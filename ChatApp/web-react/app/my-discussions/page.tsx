"use client";

import AddDiscussionButton from "@/app/ui/add-discussion-button";
import Modal from "@/app/ui/modal";
import { FormEvent, useEffect, useState } from "react";
import { createDiscussion } from "@/app/lib/actions";
import useSWR, { useSWRConfig } from "swr";
import DiscussionLink from "@/app/ui/discussion-link";

const fetcher = (...args) => fetch(...args).then((res) => res.json());

export default function Page() {
    const [showModal, setShowModal] = useState(false);
    const { data, error, isLoading, mutate } = useSWR(
        "/api/my-discussions",
        fetcher
    );

    const action = async (formData: FormData) => {
        await createDiscussion(formData);

        mutate();
    };

    if (error || isLoading) {
        return;
    }

    const discussionLinks = [];

    for (let i = 0; i < data.result.length; i++) {
        discussionLinks.push(
            <DiscussionLink
                id={data.result[i].id}
                name={data.result[i].name}
                key={i}
            />
        );
    }

    return (
        <main className="h-full">
            <div className="flex flex-wrap gap-8 items-center justify-center h-full">
                <AddDiscussionButton onClick={() => setShowModal(true)} />
                {discussionLinks}
            </div>

            <Modal isVisible={showModal} onClose={() => setShowModal(false)}>
                <h3 className="font-bold text-2xl">Create Discussion</h3>
                <form
                    className="flex flex-col"
                    action={action}
                    onSubmit={() => setShowModal(false)}
                >
                    <div className="flex flex-col mt-4">
                        <label className="text-lg" htmlFor="name">
                            Discussion Name:
                        </label>
                        <input
                            className="mt-2 p-2 text-black rounded"
                            type="text"
                            id="name"
                            name="name"
                            placeholder="Name goes here..."
                        />
                    </div>

                    <button
                        className="place-self-end mt-4 bg-zinc-700 p-2 rounded"
                        type="submit"
                    >
                        Create
                    </button>
                </form>
            </Modal>
        </main>
    );
}
