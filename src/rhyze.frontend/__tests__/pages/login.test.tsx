import React from 'react';
import { mount } from 'enzyme';
import Login from '../../pages/login';

describe('login page', () => {
    const wrapper = mount(<Login />);

    test('renders an email input', () => {
        const email = wrapper.find('input#email');

        expect(email.length).toBe(1);
    });

    test('renders a password input', () => {
        const pwd = wrapper.find('input#password');

        expect(pwd.length).toBe(1);
    });

    test('renders a login button', () => {
        const btn = wrapper.find('button');

        expect(btn.prop('type')).toBe('submit');
    });
});