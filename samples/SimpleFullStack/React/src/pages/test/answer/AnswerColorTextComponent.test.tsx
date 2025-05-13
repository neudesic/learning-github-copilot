import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import ColorTextComponent from '../ColorTextComponent';

describe('ColorTextComponent', () => {
    it('displays the amount in green when amount is positive', () => {
        render(<ColorTextComponent amount={10} />);
        const textElement = screen.getByText('10');
        expect(textElement).toBeInTheDocument();
        expect(textElement).toHaveStyle({ color: 'rgb(0, 128, 0)' });
    });

    it('displays the amount in red when amount is negative', () => {
        render(<ColorTextComponent amount={-5} />);
        const textElement = screen.getByText('-5');
        expect(textElement).toBeInTheDocument();
        expect(textElement).toHaveStyle({ color: 'rgb(255, 0, 0)' });
    });

    it('displays the amount in gray when amount is zero', () => {
        render(<ColorTextComponent amount={0} />);
        const textElement = screen.getByText('0');
        expect(textElement).toBeInTheDocument();
        expect(textElement).toHaveStyle({ color: 'rgb(128, 128, 128)' });
    });
});
